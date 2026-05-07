using System;

namespace Application.Abstractions;
// interface que su trabajo es agrupar varias operaciones de base de datos en una sola transaccion 
public interface IUnitOfWork
{
    IProduct Products { get; }
    // guarda todos los cambios pendientes en la base de datos de una sola vez
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    // funcion que lista lops pasos necesarios y si alguno da error, se revierte todo y se cancela la transaccion

    Task ExecuteInTransactionAsync(Func<CancellationToken, Task> action, CancellationToken ct = default);
}
