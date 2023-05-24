
namespace MeDirectApiProject.Helpers
{
    public static class AllureHelper
    {
        public static void ClearAllureReportsFolder()
        {
            string allureReportsPath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "..\\bin\\Debug\\net6.0\\allure-results");

            // Check if the Allure reports folder exists
            if (Directory.Exists(allureReportsPath))
            {
                // Create a DirectoryInfo object for the Allure reports folder
                DirectoryInfo directoryInfo = new DirectoryInfo(allureReportsPath);

                // Get the list of files in the Allure reports folder
                FileInfo[] files = directoryInfo.GetFiles();

                // Iterate through each file in the Allure reports folder
                foreach (FileInfo file in files)
                {
                    if (file.Name != "executor.json" && file.Name != "environment.xml")
                    {
                        file.Delete();
                    }
                }
            }
        }
    }
}
