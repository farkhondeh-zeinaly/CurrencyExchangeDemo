﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Application.Abstraction
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}
