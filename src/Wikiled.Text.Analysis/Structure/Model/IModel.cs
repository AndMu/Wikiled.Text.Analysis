﻿using System.Threading;
using System.Threading.Tasks;

namespace Wikiled.Text.Analysis.Structure.Model
{
    public interface IModel
    {
        Task Train(DataSet dataSet, CancellationToken token);
    }
}
