namespace Effect.IO;

public static class Sender<RT> where RT : struct, HasSender<RT>
{
    public static Eff<RT, Unit> SendEff(PID target, object message) =>
        from sender in default(RT).SenderEff
        from _1 in Eff(fun(() => sender.Send(target, message)))
        select unit;

    public static Aff<RT, T> RequestAff<T>(PID target, object message) =>
        from sender in default(RT).SenderEff
        from _1 in Aff(() => sender.RequestAsync<T>(target, message).ToValue())
        select _1;

}
