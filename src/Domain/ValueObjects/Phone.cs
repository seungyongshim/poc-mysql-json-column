namespace Domain.ValueObjects;


public readonly record struct Phone(
    Region Region,
    Number Number
)
{
    public string Value
    {
        init => Region = new(value);
    }
};


//public readonly record struct Phone(string Value);
