using DbDeJuguete.Models;
using DbDeJuguete.Services;

namespace DbDeJuguete;

internal abstract class Program
{
    private static List<Option> _mainOptions = null!;
    private static List<Option> _breadCrumbsOptions = null!;
    private static List<Option> _currentOptions = null!;
    private static bool _showDatabase;
    private static string _currentDbName = string.Empty;

    private static readonly string ProductBasePath =
        Path.Combine(Directory.GetCurrentDirectory(), "database", "products");

    private static readonly string RevenueBasePath =
        Path.Combine(Directory.GetCurrentDirectory(), "database", "revenue");

    private static readonly DatabaseService DatabaseProductService = new(new ProductSerializerService());
    private static readonly DatabaseService DatabaseRevenueService = new(new RevenueSerializerService());
    private static DatabaseService? _currentDatabaseService;

    public static void Main()
    {
        _mainOptions = new List<Option>
        {
            new("Mostrar Bases de Datos de Productos", ShowProductDatabases),
            new("Mostrar Bases de Datos de Ventas", ShowRevenueDatabases),
            new("Exit", () => Environment.Exit(0)),
        };
        _currentOptions = _mainOptions;
        var index = 0;

        ConsoleKeyInfo keyinfo;
        do
        {
            Console.Clear();
            if (_showDatabase) _currentDatabaseService?.PrintDatabase();
            WriteMenu(_currentOptions, _currentOptions[index]);

            keyinfo = Console.ReadKey();
            switch (keyinfo.Key)
            {
                case ConsoleKey.DownArrow:
                {
                    if (index + 1 < _currentOptions.Count)
                    {
                        index++;
                        WriteMenu(_currentOptions, _currentOptions[index]);
                    }

                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(_currentOptions, _currentOptions[index]);
                    }

                    break;
                }
                // Handle different action for the option
                case ConsoleKey.Enter:
                    _currentOptions[index].Selected.Invoke();
                    index = 0;
                    break;
            }
        } while (keyinfo.Key != ConsoleKey.X);

        Console.ReadKey();
    }

    private static void WriteMenu(List<Option> opts, Option selectedOption)
    {
        foreach (var option in opts)
        {
            Console.Write(option == selectedOption ? "> " : " ");

            Console.WriteLine(option.Name);
        }
    }

    private static void ShowProductDatabases()
    {
        _currentDatabaseService = DatabaseProductService;
        Directory.CreateDirectory(ProductBasePath);
        var dir = new DirectoryInfo(ProductBasePath);
        var fd = dir.GetFiles("*.db");
        var files = fd.Select(f => Path.GetFileNameWithoutExtension(f.FullName)).ToList();

        var options = files.Select(file =>
            new Option(file, () => { LoadProductDatabase(file); })).ToList();
        options.Add(new Option("Agregar nueva base de datos.", CreateProductDatabase));
        options.Add(new Option("Salir", () =>
        {
            _currentOptions = _mainOptions;
            _breadCrumbsOptions = _mainOptions;
        }));

        _currentOptions = options;
        _breadCrumbsOptions = options;
        WriteMenu(_currentOptions, _currentOptions[0]);
    }

    private static void ShowRevenueDatabases()
    {
        _currentDatabaseService = DatabaseRevenueService;
        Directory.CreateDirectory(RevenueBasePath);
        var dir = new DirectoryInfo(RevenueBasePath);
        var fd = dir.GetFiles("*.db");
        var files = fd.Select(f => Path.GetFileNameWithoutExtension(f.FullName)).ToList();

        var options = files.Select(file =>
            new Option(file, () => { LoadRevenueDatabase(file); })).ToList();
        options.Add(new Option("Agregar nueva base de datos.", CreateRevenueDatabase));
        options.Add(new Option("Salir", () =>
        {
            _currentOptions = _mainOptions;
            _breadCrumbsOptions = _mainOptions;
        }));

        _currentOptions = options;
        _breadCrumbsOptions = options;
        WriteMenu(_currentOptions, _currentOptions[0]);
    }

    private static void CreateProductDatabase()
    {
        Console.Clear();
        Console.WriteLine("Escriba el nombre de la nueva base de datos: ");
        var filePath = Console.ReadLine() ?? "database";
        _currentDbName = filePath;
        DatabaseProductService.CreateDatabase();
        DatabaseProductService.SaveDatabase(_currentDbName);
    }

    private static void CreateRevenueDatabase()
    {
        Console.Clear();
        Console.WriteLine("Escriba el nombre de la nueva base de datos: ");
        var filePath = Console.ReadLine() ?? "database";
        _currentDbName = filePath;
        DatabaseRevenueService.CreateDatabase();
        DatabaseRevenueService.SaveDatabase(_currentDbName);
    }

    private static void LoadProductDatabase(string file)
    {
        DatabaseProductService.LoadDatabase(file);
        _currentDbName = file;
        _showDatabase = true;

        var options = new List<Option>
        {
            new("Agregar registro", () => { DatabaseProductService.Create(); }),
            new("Actualizar registro", () => { DatabaseProductService.Update(GetItemIdFromClient()); }),
            new("Eliminar registro", () => { DatabaseProductService.Delete(GetItemIdFromClient()); }),
            new("Salir", () =>
            {
                DatabaseProductService.SaveDatabase(_currentDbName);
                _currentOptions = _breadCrumbsOptions;
                _showDatabase = false;
            })
        };
        _currentOptions = options;
        WriteMenu(_currentOptions, _currentOptions[0]);
    }

    private static void LoadRevenueDatabase(string file)
    {
        DatabaseRevenueService.LoadDatabase(file);
        _currentDbName = file;
        _showDatabase = true;

        var options = new List<Option>
        {
            new("Agregar registro", () => { DatabaseRevenueService.Create(); }),
            new("Actualizar registro", () => { DatabaseRevenueService.Update(GetItemIdFromClient()); }),
            new("Eliminar registro", () => { DatabaseRevenueService.Delete(GetItemIdFromClient()); }),
            new("Salir", () =>
            {
                DatabaseRevenueService.SaveDatabase(_currentDbName);
                _currentOptions = _breadCrumbsOptions;
                _showDatabase = false;
            })
        };
        _currentOptions = options;
        WriteMenu(_currentOptions, _currentOptions[0]);
    }

    private static int GetItemIdFromClient()
    {
        Console.WriteLine("Escribe el Id del elemento a actualizar: ");
        if (!int.TryParse(Console.ReadLine(), out var itemId)) return 0;
        return itemId;
    }
}