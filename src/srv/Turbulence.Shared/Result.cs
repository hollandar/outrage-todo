using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbulence.Shared
{
    public class Result
    {
        bool success;
        string? error;

        public Result(bool success, string? error = null)
        {
            this.success = success;
            this.error = error;
        }

        public bool Success { get => success; set 
            {
                this.success = value;
                this.error = value ? null: this.error ?? "An error occurred.";
            } 
        }

        public string? Error { get => error; set => error = value; }

        public Result() : this(true) { }
        public Result(string error) : this(false, error) { }

        public static implicit operator Result(bool success) => new Result(success, success ? null: "An error occurred.");
        public static implicit operator Result(string error) => new Result(false, error);
        public static Result Fail(string error) => error;
        public static Result Ok() => true;
    }
}
