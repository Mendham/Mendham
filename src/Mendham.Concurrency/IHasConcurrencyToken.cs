﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Concurrency
{
	public interface IHasConcurrencyToken
	{
        IConcurrencyToken Token { get; set; }
    }
}
