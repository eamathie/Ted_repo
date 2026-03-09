
namespace tedMovieApp.Tools;

public static class DotEnvLoader
{
    public static void Load(string filePath)
    {
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($".env file not found at {filePath}");

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }

        // verifying all env keys are set
        string[] requiredEnvs = ["DB_HOST", "DB_NAME", "DB_USER", "DB_PASS", "JWT_KEY", "JWT_ISSUER", "JWT_AUDIENCE"];
        var missing = requiredEnvs.Where(env =>
            string.IsNullOrEmpty(Environment.GetEnvironmentVariable(env))).ToList();
        if (missing.Count > 0)
            missing.ForEach(envVar => throw new InvalidOperationException("Missing env variables: " + string.Join(",", envVar)));
    }

    public static string GenerateConnectionString()
    {
        
        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
        
        return $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPass}";
    }
}