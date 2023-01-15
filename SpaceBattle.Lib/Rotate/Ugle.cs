namespace SpaceBattle.Lib;

public class Ugle {
    public int ch = -1;
    public int z = -1;
    public Ugle(){ }
    public Ugle(int a, int b){
        if(b<0){ 
            b=Math.Abs(b); 
            a*=-1; 
        }
        while(a<0){
            a+=360*b;
        }
        if(a>360*b){
            a%=360*b;
        }
        for(int i=2; i<19;i++){
            if(b%i==0 && a%i==0){
                a/=i;
                b/=i;
                i=2;
            }
        }
        this.ch = a;
        this.z = b;
    }
    public static Ugle operator +(Ugle a, Ugle b)
    {
        if (a.ch == -1 || a.z == -1 || b.ch == -1 || b.z == -1) throw new ArgumentException();
        int[] c = new int[2];
        c[0] = (a.ch*b.z)+(b.ch*a.z);
        c[1] = a.z*b.z;
        return new Ugle(c[0], c[1]);
    }
    /*public static bool operator ==(Ugle a, Ugle b)
    {
        if (a.ch == -1 || a.z == -1 || b.ch == -1 || b.z == -1) throw new ArgumentException();
        if(a.ch*b.z==b.ch*a.z) return true;
        else return false;
    }
    public static bool operator !=(Ugle a, Ugle b)
    {
        return !(a == b);
    }*/

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;

        if (ReferenceEquals(obj, null)) return false;

        Ugle ugle2 = (Ugle) obj;
        bool a = ((ugle2.ch == this.ch) && (ugle2.z == this.z));
        if (this.GetHashCode() == ugle2.GetHashCode()) {
            return a;
        } else {
            return false;
        }
    }
    
    public override int GetHashCode()
    {
        return ("" + this.ch + ":" + this.z).GetHashCode();
    }
}
