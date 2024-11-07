using System.Runtime.CompilerServices;

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
        public Dictionary<string, List<string>> linkingCategories;
        public Dictionary<string, string> specificToGeneral; //backwards linking
        
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
            specificToGeneral = new Dictionary<string, string>();
            linkingCategories = new Dictionary<string,  List<string>>();
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
        public void ReMap(string newGenCategory, string specific, double amount, bool reassigning)
        {
            if (reassigning)
            {
                if (specificToGeneral.TryGetValue(specific, out var oldCategory))
                {
                    linkingCategories.TryGetValue(oldCategory, out List<string> OldSpecificList);
                    OldSpecificList.Remove(specific);
                    //reassign this specific variable to the new category
                    oldCategory = newGenCategory;
                    //linking categories hasnt added this new expense specific
                    Spend(newGenCategory, specific, amount);
                }
            }
            else
            {
                if (specificToGeneral.TryGetValue(specific, out var oldCategory))
                {
                    //they just chose the old category, dont need to remap, just recall spend with a new category
                    Spend(oldCategory, specific, amount);
                }
                    
            }
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
            //this category exists in the linking dictionary
            if (linkingCategories!.TryGetValue(categoryGeneral, out List<string> sepcificsOut))
            {
                //add this specific expense to the list of expenses for this general category
                if(sepcificsOut != null)
                {
                    //if this specific category isnt mapped to any amount we can add it no problem
                    if (!specifics.ContainsKey(categorySpecific)){
                        sepcificsOut.Add(categorySpecific);
                    }
                    //THIS HANDLES THE SAME SPECIFIC CATEGORY IN TWO DIFFERENT CATEGORIES
                    //this specific category is mapped to an amount, lets see if it is under this category
                    else if (!sepcificsOut.Contains(categorySpecific))
                    {
                        //oh no we used this specific category in another broad category!
                        //return false and continue...
                        //NOTHING HAPPENS - RECALL THIS METHOD WITH A MODIFICATION
                        //either we will re-map this specific and amount to a new general category (remove from the specifics
                        //list in the old category and add to the specifics list in the new category - dont add new amount yet)
                        return false;
                    }
                    else
                    {
                        //specifics list contains this. we willnot add to list
                    }

                }
                // return false;
            }
            else if (specificToGeneral.ContainsKey(categorySpecific))
            {
                return false;
            }
            //this category has zero specifics so far. lets start a list!
            else
            {
                linkingCategories.Add(categoryGeneral, new List<string> {categorySpecific});
                specificToGeneral.Add(categorySpecific, categoryGeneral);
                // return true;
            }

            

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
                //return false;
            }
            else
            {

                specifics.Add(categorySpecific, amount);
                
                //specific added
                //return true;
            }
            //update the budget left
            BudgetLeft -= amount;
            return true;
        }


        

    }
}
