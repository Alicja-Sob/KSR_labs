using Messages_etc;
public class Warehouse_state
{
    public int Aviable_items { get; set; } = 0;
    public int Reserved_items { get; set; } = 0;

    public void printAmounts()
    {
        Custom_ConsoleCol.ConsoleWrite($"---------------------------------", ConsoleColor.Red);
        Custom_ConsoleCol.ConsoleWrite($"         WAREHOUSE STOCK        ", ConsoleColor.Red);
        Custom_ConsoleCol.ConsoleWrite($"Aviable items: {Aviable_items}", ConsoleColor.Red);
        Custom_ConsoleCol.ConsoleWrite($"Reserved items: {Reserved_items}", ConsoleColor.Red);
        Custom_ConsoleCol.ConsoleWrite($"---------------------------------", ConsoleColor.Red);
    }
}