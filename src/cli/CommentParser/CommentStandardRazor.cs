﻿using Common;

namespace CommentParser;

public class CommentStandardRazor : CommentStandard
{
    static Globs globs = new Globs("**/*.razor");
    static List<StringLike> singleline = new();
    static List<(StringLike start, StringLike end)> multiline = new() { ("@*", "*@") };

    public override Globs Globs => globs;
    public override IEnumerable<StringLike> Singleline => singleline;
    public override IEnumerable<(StringLike start, StringLike end)> Multiline => multiline;
}