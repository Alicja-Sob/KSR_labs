using System.Text;

public static class cipher_rot13
{
    public static string Encode_Rot13(string content)
    {
        var result = new StringBuilder();
        foreach (char letter in content)
        {
            //https://en.wikipedia.org/wiki/ROT13#Python
            if (letter >= 'a' && letter <= 'z')
                result.Append((char)('a' + (letter - 'a' + 13) % 26));
            else if (letter >= 'A' && letter <= 'Z')
                result.Append((char)('A' + (letter - 'A' + 13) % 26));
            else
                result.Append(letter);
        }

        return result.ToString();
    }
}