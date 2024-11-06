namespace BlazorLogic
{
    /// <summary>
    /// 
    /// this class holds the data entered by the user in the app. 
    /// User enters a 
    ///     -total budget
    ///     -name/category of cost
    ///     -amount spent in that category
    /// We will keep track of
    ///     -budgetLeft(double)
    ///     -category(string) mapped to a totaly money amount(double) - this is also displayed under the input
    /// </summary>
    public class Budget
    {
        public double BudgetLeft;
        private Dictionary<string, double> categories;//general categories:food, gas, misc, clothes, rent, ect
        private Dictionary<string, double> specifics;//more specifics: movies, cafe rio, electric bill, ect
        private List<string> categoriesList;

        public Dictionary<string, double> GetCategories()
        {
            return categories;
        }
        public Dictionary<string, double> GetSpecifics()
        {
            return specifics;
        }

        public Budget(double totalBudget)
        {
            BudgetLeft = totalBudget;
            categories = new Dictionary<string, double>();
            specifics = new Dictionary<string, double>();
            categoriesList = new List<string>();
            categories.Add("food", 0);
            categoriesList.Add("food");
            categories.Add("personal", 0);
            categoriesList.Add("personal");
            categoriesList.Add("rent");
            categoriesList.Add("gas");
            categoriesList.Add("savings");
            categories.Add("rent", 0);
            categories.Add("gas", 0);
            categories.Add("savings", 0);
            categories.Add("new category", 0);
            categoriesList.Add("new category");


        }

        ///returns the list of categories currently in the dictionary
        public List<string> getCategories()
        {
            
            return this.categoriesList;
        }
        /// <summary>
        /// 
        /// return true if there is a new category added
        /// </summary>
        /// <param name="category"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool Spend(string categoryGeneral, string categorySpecific, double amount)
        {
            //update the budget left
            BudgetLeft -= amount;

            //BROAD CATEGORIES FIRST
            //this category exists in the dictionary
            if(categories!.TryGetValue(categoryGeneral,  out double currentTotal)){
                categories[categoryGeneral] = currentTotal + amount;
               // return false;
            }
            else
            {
                categories.Add(categoryGeneral, amount);
                categoriesList.Add(categoryGeneral);
               // return true;
            }

            //HANDLE SPECIFICS
            if (specifics!.TryGetValue(categorySpecific, out double currentSpecifics))
            {
                specifics[categorySpecific] = currentSpecifics+ amount;
                //no new specific added
                return false;
            }
            else
            {

                specifics.Add(categorySpecific, amount);
                
                //specific added
                return true;
            }

        }


        

    }
}
