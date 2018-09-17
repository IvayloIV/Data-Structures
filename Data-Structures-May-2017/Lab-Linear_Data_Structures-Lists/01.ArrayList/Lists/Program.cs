public class Program
{
    public static void Main(string[] args)
    {
        var arrayList = new ArrayList<int>();
        arrayList.Add(1);
        arrayList.Add(2);
        arrayList.Add(3);
        arrayList.Add(4);
        arrayList.RemoveAt(0);
        arrayList.RemoveAt(0);
        arrayList.RemoveAt(0);
    }
}
