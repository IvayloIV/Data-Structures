using System;

namespace Ropes_and_Tries
{
    class Program
    {
        static void Main(string[] args)
        {
            var textEditor = new TextEditor();
            string command;
            while ((command = Console.ReadLine()) != "end")
            {
                var tokens = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                DoCommand(textEditor, command, tokens);
            }
        }

        private static void DoCommand(TextEditor textEditor, string command, string[] tokens)
        {
            if (tokens[0] == "login")
            {
                textEditor.Login(tokens[1]);
            }
            else if (tokens[0] == "logout")
            {
                textEditor.Logout(tokens[1]);
            }
            else if (tokens[0] == "users")
            {
                Console.WriteLine(String.Join(Environment.NewLine, textEditor.Users(tokens.Length == 2 ? tokens[1] : "")));
            }
            else if (tokens[1] == "insert")
            {
                var text = command.Split(new string[] { "\"" }, StringSplitOptions.None);
                textEditor.Insert(tokens[0], int.Parse(tokens[2]), text[1]);
            }
            else if (tokens[1] == "prepend")
            {
                var text = command.Split(new string[] { "\"" }, StringSplitOptions.None);
                textEditor.Prepend(tokens[0], text[1]);
            }
            else if (tokens[1] == "undo")
            {
                textEditor.Undo(tokens[0]);
            }
            else if (tokens[1] == "substring")
            {
                textEditor.Substring(tokens[0], int.Parse(tokens[2]), int.Parse(tokens[3]));
            }
            else if (tokens[1] == "clear")
            {
                textEditor.Clear(tokens[0]);
            }
            else if (tokens[1] == "delete")
            {
                textEditor.Delete(tokens[0], int.Parse(tokens[2]), int.Parse(tokens[3]));
            }
            else if (tokens[1] == "length")
            {
                Console.WriteLine(textEditor.Length(tokens[0]));
            }
            else if (tokens[1] == "print")
            {
                Console.WriteLine(textEditor.Print(tokens[0]));
            }
        }
    }
}
