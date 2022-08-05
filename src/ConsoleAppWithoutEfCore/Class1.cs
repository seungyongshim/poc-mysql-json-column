using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.ValueObjects;
using Microsoft.VisualBasic;

namespace ConsoleAppWithoutEfCore;


public interface IGeneralRepository<T>
{

    Task UpsertAsync(T value);
}
