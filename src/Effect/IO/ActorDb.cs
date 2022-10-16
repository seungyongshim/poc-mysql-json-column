namespace Effect.IO;

public static class ActorDb<RT> where RT : struct, HasDb<RT>
{
    public static Aff<RT, Unit> UpsertAff(string key, object value, string tableName) =>
        from db in default(RT).DbEff
        from ret in Aff(() => db.UpsertAsync(key, value, tableName).ToValue()).Retry(Max30Sec)
        select unit;

    public static Aff<RT, object> FindByIdAff(string key, string tableName) =>
        from db in default(RT).DbEff
        from ret in Aff(() => db.FindByIdAsync(key, tableName).ToValue()).Retry(Max30Sec)
        select ret;
}
