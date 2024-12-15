using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price", "SpecialTag" },
                values: new object[,]
                {
                    { 1, "Категория 2", "Управление по таким.", "https://placehold.co/200", "Лоснящийся Хлопковый Кошелек", 465.44, "Популярный" },
                    { 2, "Категория 3", "Укрепления прогресса нас экономической понимание образом профессионального отношении определения.", "https://placehold.co/200", "Свободный Кожанный Портмоне", 84.480000000000004, "Популярный" },
                    { 3, "Категория 2", "Направлений новых с повседневная определения определения для гражданского рамки.", "https://placehold.co/200", "Лоснящийся Пластиковый Майка", 511.83999999999997, "Рекомендуемый" },
                    { 4, "Категория 3", "Обеспечивает повседневной на в место структуры.", "https://placehold.co/200", "Невероятный Натуральный Ремень", 777.40999999999997, "Рекомендуемый" },
                    { 5, "Категория 3", "Насущным задач предпосылки модели активом кругу обществом таким материально-технической качества.", "https://placehold.co/200", "Потрясающий Пластиковый Клатч", 524.60000000000002, "Новинка" },
                    { 6, "Категория 2", "Участниками консультация создание массового воздействия.", "https://placehold.co/200", "Лоснящийся Резиновый Ножницы", 801.92999999999995, "Новинка" },
                    { 7, "Категория 2", "Прогресса качества значительной важную количественный позволяет структуры общественной различных.", "https://placehold.co/200", "Эргономичный Меховой Шарф", 569.33000000000004, "Популярный" },
                    { 8, "Категория 1", "Поставленных и вызывает.", "https://placehold.co/200", "Фантастический Натуральный Ножницы", 24.690000000000001, "Рекомендуемый" },
                    { 9, "Категория 1", "Систему собой насущным.", "https://placehold.co/200", "Невероятный Стальной Автомобиль", 130.16999999999999, "Новинка" },
                    { 10, "Категория 3", "Постоянное значимость рамки внедрения.", "https://placehold.co/200", "Интеллектуальный Хлопковый Кошелек", 994.25, "Рекомендуемый" },
                    { 11, "Категория 2", "Участниками богатый также.", "https://placehold.co/200", "Интеллектуальный Деревянный Компьютер", 263.51999999999998, "Популярный" },
                    { 12, "Категория 3", "Управление плановых от требует эксперимент высокотехнологичная значение мира развития.", "https://placehold.co/200", "Эргономичный Деревянный Свитер", 822.24000000000001, "Популярный" },
                    { 13, "Категория 1", "Качественно уточнения профессионального.", "https://placehold.co/200", "Интеллектуальный Бетонный Сабо", 852.88999999999999, "Новинка" },
                    { 14, "Категория 1", "Изменений курс кадров.", "https://placehold.co/200", "Практичный Стальной Майка", 287.13, "Популярный" },
                    { 15, "Категория 2", "Национальный шагов национальный постоянное ресурсосберегающих различных правительством.", "https://placehold.co/200", "Фантастический Гранитный Ботинок", 721.5, "Популярный" },
                    { 16, "Категория 3", "Постоянное забывать создаёт качества потребностям а актуальность.", "https://placehold.co/200", "Лоснящийся Деревянный Кошелек", 336.81, "Рекомендуемый" },
                    { 17, "Категория 2", "Обуславливает соответствующих активизации прежде повседневная.", "https://placehold.co/200", "Лоснящийся Меховой Берет", 303.07999999999998, "Рекомендуемый" },
                    { 18, "Категория 2", "Проект анализа предложений значительной также структуры значительной.", "https://placehold.co/200", "Большой Неодимовый Сабо", 378.94, "Популярный" },
                    { 19, "Категория 2", "Информационно-пропогандистское прогрессивного образом обучения соответствующих забывать понимание.", "https://placehold.co/200", "Интеллектуальный Меховой Кулон", 740.26999999999998, "Рекомендуемый" },
                    { 20, "Категория 2", "Обеспечивает задача порядка разработке социально-экономическое форм формировании вызывает целесообразности кадровой.", "https://placehold.co/200", "Маленький Кожанный Ботинок", 360.75999999999999, "Популярный" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
