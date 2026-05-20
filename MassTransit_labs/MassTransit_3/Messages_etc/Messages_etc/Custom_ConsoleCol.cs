namespace Messages_etc
{
    public static class Custom_ConsoleCol
    {
        public static void ConsoleWrite(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}