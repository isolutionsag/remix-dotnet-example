using LoremNET;
using System.Diagnostics.CodeAnalysis;
using todo.Core.Entities;

namespace todo.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DbInitializer
{
    public static void Init(TodoContextSqlLite context)
    {
        // provide cupcake ipsum
        Source.LoremIpsum = "Bonbon, Croissant, Candy canes, Halvah, Cupcake, Marshmallow, Topping, Cheesecake, Muffin, Candy, Pudding, Gingerbread, Lollipop, Macaroon, Sorbet, Frosting, Shortbread, Truffle, Jellybean, Brownie, Caramel, Churro, Scone, Pancake, Toffee, Eclair, Praline, Cannoli, Biscotti, Ganache, Marzipan, Pavlova, Tiramisu, Popsicle, Custard, Fudge, Sundae, Peanut brittle, Éclair, Gelato, Scone, Panna cotta, Strudel, Soufflé, Baklava, Pecan pie, Cherry turnover, Cupcake, Cinnamon roll, Chocolate chip cookie, Banana bread, Blueberry muffin, Lemon tart, Raspberry Danish, Pumpkin pie, Coconut macaroon, Key lime pie, Red velvet cake, Carrot cake, Black forest gateau, Tres leches cake, Angel food cake, Pineapple upside-down cake, Chocolate lava cake, Strawberry shortcake, Coffee cake, Buttercream, Whipped cream, Sugar glaze, Rainbow sprinkles, Chocolate shavings, Candied orange peel, Almond brittle, Maple syrup, Honey drizzle, Vanilla extract, Cocoa powder, Powdered sugar, Confectioner’s glaze, Caramelized sugar, Mint leaves, Edible flowers, Silver dragées, Gold leaf, Candied ginger, Toasted coconut, Crushed pistachios, Hazelnut praline, Berry compote, Passion fruit coulis, Lemon zest, Orange blossom water, Rosewater, Peppermint extract, Chai spices, Cinnamon dust, Nutmeg sprinkle, Cardamom pods, Lavender buds, Earl Grey tea, Espresso beans, Matcha powder, Toasted sesame seeds, Poppy seeds, Pumpkin spice, Graham cracker crumbs, Oreo crumbles, Peanut butter swirl, Salted caramel, Molasses drizzle, Butterscotch chips, Toasted marshmallow, Candied pecans, Walnut brittle, Pistachio dust, Coconut flakes, Chocolate ganache, Raspberry jam, Blueberry compote, Strawberry preserves, Apricot glaze, Fig jam, Blackberry coulis, Cherry compote, Passion fruit curd, Lemon meringue, Lime zest, Kiwi slices, Pomegranate seeds, Mango purée, Papaya chunks, Guava syrup, Lychee pearls, Dragon fruit, Starfruit slices, Pineapple rings, Watermelon cubes, Cantaloupe balls, Honeydew wedges, Coconut water, Mint sprigs, Basil leaves, Rosemary sprigs, Thyme leaves, Lavender buds, Chamomile petals, Hibiscus petals, Edible gold dust, Silver sugar pearls, Rainbow nonpareils, Chocolate curls, Candied violets, Maraschino cherries, Candied lemon peel, Candied lime zest, Candied grapefruit peel, Candied kumquat, Candied pineapple, Candied ginger, Candied rose petals, Candied lavender, Candied mint leaves, Candied basil, Candied thyme, Candied sage, Candied marjoram, Candied oregano, Candied parsley, Candied cilantro, Candied dill, Candied chives, Candied tarragon, Candied bay leaves, Candied lemon verbena, Candied lemon balm, Candied lemon basil, Candied lemon thyme".ToLower();

        RemoveData(context);
        GenerateCategories(context);
        GenerateLists(context);
        GenerateListEntries(context);
    }

    private static void GenerateListEntries(TodoContextSqlLite context)
    {
        foreach (var list in context.TodoLists)
        {
            var count = Lorem.Integer(1, 10);

            var newEntries = Enumerable.Range(0, count).Select(_ => new TodoListEntry
            {
                Description = Lorem.Sentence(3, 10),
                IsCompleted = Lorem.Integer(0, 1) == 1,
                Title = Lorem.Words(1, 3),
                ParentId = list.Id,
            });

            context.TodoListEntries.AddRange(newEntries);
        }

        context.SaveChanges();
    }

    private static void GenerateLists(TodoContextSqlLite context)
    {
        foreach (var category in context.TodoListCategories)
        {
            var list = new TodoList() { CategoryId = category.Id, Name = Lorem.Words(2, 5) };
            context.TodoLists.Add(list);
        }

        context.SaveChanges();
    }

    private static void GenerateCategories(TodoContextSqlLite context)
    {
        // add dummy data
        var categoryCedric = Guid.Parse("71e6d95e-aa65-4048-beee-22f9da22006e");
        var categoryGianLuca = Guid.Parse("a8d5ba61-bf91-47c9-b959-c6c1b8bc4e50");
        var categoryRegina = Guid.Parse("fa335f10-1690-43e3-be74-a5a64892cbc6");

        context.TodoListCategories.Add(new() { Id = categoryCedric, Name = "Cedrics State of Happiness" });
        context.TodoListCategories.Add(new() { Id = categoryGianLuca, Name = "Gian-Lucas Happy Place" });
        context.TodoListCategories.Add(new() { Id = categoryRegina, Name = "Reginas Paradise" });


        context.SaveChanges();
    }

    private static void RemoveData(TodoContextSqlLite context)
    {
        var entries = context.TodoListEntries.ToList();
        context.TodoListEntries.RemoveRange(entries);

        var lists = context.TodoLists.ToList();
        context.TodoLists.RemoveRange(lists);

        var categories = context.TodoListCategories.ToList();
        context.TodoListCategories.RemoveRange(categories);

        context.SaveChanges();
    }
}