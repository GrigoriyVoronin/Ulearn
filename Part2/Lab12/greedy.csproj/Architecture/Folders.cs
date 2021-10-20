using System;
using System.IO;

namespace Greedy.Architecture
{
    public static class Folders
    {
        public static readonly DirectoryInfo StatesForStudents = new DirectoryInfo(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "states-for-students"));

        public static readonly DirectoryInfo NotGreedyStatesForStudents =
            new DirectoryInfo(Path.Combine(StatesForStudents.FullName, "not-greedy-states"));
    }
}