namespace SpaceBattle.Lib;
public class Vector
{
    public int[] coordinates;
    public int Size;
    public Vector(params int[] args)
    {
        coordinates = args;
        Size = coordinates.Length;
    }
    public int this[int i]
    {
        get
        {
            return coordinates[i];
        }
        set
        {
            coordinates[i] = value;
        }
    }
    public static Vector operator +(Vector a, Vector b)
    {
        if (a.Size != b.Size) throw new ArgumentException();
        int[] array = new int[a.Size];
        for (int i = 0; i < a.Size; i++) array[i] = a[i] + b[i];

        return new Vector(array);
    }
    public static Vector operator -(Vector a, Vector b)
    {
        if (a.Size != b.Size) throw new ArgumentException();
        int[] array = new int[a.Size];
        for (int i = 0; i < a.Size; i++) array[i] = a[i] - b[i];

        return new Vector(array);
    }
    public static bool operator ==(Vector a, Vector b)
    {
        if (a.Size != b.Size) return false;
        for (int i = 0; i < a.Size; i++)
            if (a[i] != b[i]) return false;
        return true;
    }
    public static bool operator !=(Vector a, Vector b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj) => obj is Vector v && coordinates.SequenceEqual(v.coordinates);

    public override int GetHashCode()
    {
        return String.Join("", coordinates.Select(x => x.ToString())).GetHashCode();
    }
}
