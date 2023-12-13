using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbulence.Shared
{
    public class ValueResult<TType>: Result
    {
        TType? value;

        public ValueResult(TType value): base()
        {
            this.value = value;
        }

        public ValueResult(): base()
        {
            
        }

        public TType Value { get => value; set => this.value = value; }

        public ValueResult(string error): base(false, error)
        {
            value = default(TType);
        }

        public static implicit operator ValueResult<TType>(TType success) => new ValueResult<TType>(success);
        public static implicit operator ValueResult<TType>(string error) => new ValueResult<TType>(error);
        public static ValueResult<TValue> Fail<TValue>(string error) => error;
        public static ValueResult<TValue> Ok<TValue>(TValue value) => value;
    }
}
