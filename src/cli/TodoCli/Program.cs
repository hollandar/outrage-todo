// See https://aka.ms/new-console-template for more information
using CommentDestructure;
using CommentParser;
using Common;
using ConsoleApp1;
using ConsoleApp1.Settings;
using ExpressionParser;
using GlobExpressions;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Webefinity.Switch;

var argumentsBuilder = new ArgumentsBuilder();
argumentsBuilder.Add("path", 'p', true).AcceptDirectory(true).MakeRequired();
argumentsBuilder.Add("settings", 's').AcceptFilename(true).MakeRequired();

Console.WriteLine(Directory.GetCurrentDirectory());

var argumentsHandler = argumentsBuilder.Build();

if (argumentsHandler.IsValid)
{
    var settingsFile = argumentsHandler.GetValue<string>("settings");
    var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    serializerOptions.Converters.Add(new JsonStringEnumConverter());
    var settings = JsonSerializer.Deserialize<Root>(File.ReadAllText(settingsFile!), serializerOptions) ?? new Root();
    var path = argumentsHandler.GetValue<string>("path");

    var files = Directory.GetFiles(path!, "*", new EnumerationOptions { RecurseSubdirectories = true });
    var finder = new CommentFinder();

    var languageStandards = settings.Languages.Select(r => r.ToStandard()).ToList();
    foreach (var customLanguage in settings.CustomLanguages)
    {
        var singleLine = customLanguage.Singleline.Select(r => (StringLike)new IsString(r)).Union(customLanguage.NotSingleline.Select(r => (StringLike)new IsNotString(r)));
        var multiLine = customLanguage.Multiline.Select(r => ((StringLike)new IsString(r.Start), (StringLike)new IsString(r.End)));
        languageStandards.Add(new CommentStandardCustom(new Globs(customLanguage.Globs.ToArray()), singleLine.ToArray(), multiLine.ToArray()));
    }
    var commentParser = new CommentParser.Parser(languageStandards.ToArray());

    var expressionStandards = settings.Expressions.Select(r => r.ToExpressionRule()).ToList();
    foreach (var customExpression in settings.CustomExpressions)
    {
        expressionStandards.Add(new ExpressionRule(customExpression.Globs, customExpression.Regex));
    }
    var expressionParser = new ExpressionParser.Parser(expressionStandards.ToArray());

    var commentTypes = new CommentTypes();
    var start = Stopwatch.GetTimestamp();
    foreach (var file in files)
    {
        if (languageStandards.Any(r => r.Globs.IsMatch(file)))
        {
            var content = File.ReadAllText(file);
            var comments = commentParser.Parse(file, content);
            if (comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    var commentType = commentTypes.Match(comment.Content);
                    if (commentType is not null)
                    {
                        var destructuredComment = new DestructuredComment(commentType, comment.Content, comment.Line);
                        Console.WriteLine("### " + file + "@" + destructuredComment.Line + " ## " + destructuredComment.CommentType);
                        Console.WriteLine(destructuredComment.Text);

                        foreach (var parameter in destructuredComment.Parameters)
                        {
                            Console.WriteLine($" - {parameter.Key}: {parameter.Value}");
                        }
                    }
                }
            }

            var expressions = expressionParser.Parse(file, content);
            if (expressions.Count > 0) { 
                foreach (var expression in expressions)
                {
                    Console.WriteLine("### " + file + "@" + expression.Line);
                    Console.WriteLine(expression.Content);
                }
            }

            
        }
    }

    Console.WriteLine(Stopwatch.GetElapsedTime(start).TotalMilliseconds);

    Console.ReadLine();
}
else
{
    argumentsHandler.Usage("ConsoleApp1", "1.0.0");
    Console.ReadLine();
}

