using System;
using System.Collections.Generic;
using System.Text;
using C971_WGU_App.Models;
using SQLite;

namespace C971_WGU_App
{
    public class DatabaseHandler
    {
        private SQLiteConnection conn;

        public DatabaseHandler(string dbPath)
        {
            conn = new SQLiteConnection(dbPath);
            string commandText = "PRAGMA foreign_keys = ON";
            var foreignKeyCommand = conn.CreateCommand(commandText);
            foreignKeyCommand.ExecuteNonQuery();
        }

        public void CreateTables()
        {
            conn.CreateTable<Term>();
            conn.CreateTable<Course>();
            conn.CreateTable<Instructor>();
            conn.CreateTable<Assessment>();
        }

    }
}
