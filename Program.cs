using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SkillDiagBackend2021_task_3
{
    class Program
    {

        /*
         
             Задача 3 на скилл диагностике Яндекс 2021

           Всем хотя бы раз в жизни приходилось перекладывать JSON. Вот и для нового проекта под названием «Единое хранилище» необходимо переложить магазинные фиды. 
        Для размещения на Яндекс.Маркете магазины передают товары из своего ассортимент в JSON-файлах. Одно товарное предложение описывается так:

                {  
                    "offer_id": <string>,  
                    "market_sku": <int>,  
                    "price": <int>  
                }

        где  offer_id — уникальный среди всех фидов магазина идентификатор предложения, 
             market_sku — идентификатор товара на Яндекс.Маркете, 
             price — стоимость товара.

        Весь фид магазина представляет собой JSON и выглядит так:

                {  
                    "offers": [<offer>, <offer>, ...]  
                }
        Вас попросили написать программу, которая объединит все фиды одного магазина в единый фид и отсортирует товары в порядке неубывания их стоимостей, а при их равенстве — по offer_id.

        Формат ввода
        В первой строке входных данных содержится целое число n — количество фидов магазина (1≤n≤200). Следующие n строк содержат по одному магазинному фиду на строку. 
        Гарантируется, что строка — валидный JSON и удовлетворяет формату фида. В одном фиде не больше 200 товарных предложений. offer_id состоит из строчных и заглавных букв латинского алфавита и цифр, 
        1≤|offer_id|≤10, 1≤market_sku≤231−1, 1≤price≤106.
        Формат вывода
        Выходной поток должен содержать один JSON-документ, удовлетворяющий формату товарного фида. Количество строк в выходном файле и табуляция не имеют значения.

        ВВОД
        2
        {"offers": [{"offer_id": "offer1", "market_sku": 10846332, "price": 1490}, {"offer_id": "offer2", "market_sku": 682644, "price": 499}]}
        {"offers": [{"offer_id": "offer3", "market_sku": 832784, "price": 14000}]}
        ВЫВОД
        {"offers":[{"market_sku":682644,"offer_id":"offer2","price":499},{"market_sku":10846332,"offer_id":"offer1","price":1490},{"market_sku":832784,"offer_id":"offer3","price":14000}]}


         Необходимо подключить Newtonsoft.Json для сборки/разборки JSON

         Задачу можно решить без LINQ но это дольше и не так красиво.


        */

        /// <summary>
        /// Модель данных предложений от магазинов
        /// </summary>
        [Serializable]
        class offer
        {
            /// <summary>
            /// Фид - уникальный среди всех фидов магазина идентификатор предложения
            /// </summary>
            public string offer_id { get; set; }

            /// <summary>
            /// Идентификатор товара на Яндекс.Маркете
            /// </summary>
            public int market_sku { get; set; }

            /// <summary>
            /// Стоимость товара
            /// </summary>
            public int price { get; set; }
        }

        static void Main(string[] args)
        {
            //Количество входных JSON
            int countInputJSONLine = Convert.ToInt32(Console.ReadLine());

            //Проверяем количество вводимых JSON строк
            if (countInputJSONLine >= 1 && countInputJSONLine <= 200)
            {
                //Массив предложений
                List<offer> offerList = new List<offer>();
                
               
                //Считываем все строки JSON
                for (int i = 0; i < countInputJSONLine; i++)
                {
                    //Разбираем JSON на массив данных
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(Console.ReadLine());
                    //Получаем данные в виде таблицы
                    DataTable dataTable = dataSet.Tables["offers"];

                    //Пробегаем по строчкам таблицы и заносим их в массив предложений
                    foreach (DataRow row in dataTable.Rows)
                    {
                        //Создаем новый экземпляр класса предложений
                        offer ofr = new offer();
                        ofr.offer_id = (string)row["offer_id"];
                        ofr.market_sku = Convert.ToInt32(row["market_sku"]);
                        ofr.price = Convert.ToInt32(row["price"]);

                        //Добавляем в общий массив предложений
                        offerList.Add(ofr);
                    }

                }

                //Сортируем массив согласно задачи при помощи LINQ Сначало цена, затем Фид
                offerList = offerList.Select(x => x).OrderBy(x => x.price).ThenBy(y => y.offer_id).ToList();//

                //Сереализуем JSON обратно в строчку
                string json = JsonConvert.SerializeObject(offerList);

                //Собераем выходную строчку
                string outJSON = "{\"offers\":";
                outJSON = outJSON + json;
                outJSON = outJSON + "}";

                //Выводим результат
                Console.WriteLine(outJSON);
            }
            else
            {
                //Выдаем ошибку
                Console.WriteLine("error");
            }


        }
    }
}
