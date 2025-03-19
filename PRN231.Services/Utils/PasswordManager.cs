//using BCrypt.Net;

using EXE101.Services.Utils;

namespace EXE101.Services.Utils;
public class PasswordManager
{
    // Function to hash a password
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Function to check password validity
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    
    /*public static void CombineCollections(List<List<Variant>> collections, int index, List<string> current, List<string> result)
    {
        if (index == collections.Count)
        {
            result.Add(string.Join(" - ", current));
            return;
        }

        var collection = collections[index];
        //var type = collection[0].VariantTypeId;
        var values = collection.Select(x => x.Name).ToList();

        foreach (var value in values)
        {
            current.Add($"{value}");
            CombineCollections(collections, index + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }*/
}
