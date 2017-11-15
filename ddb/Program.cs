using System;
using Absolut.ADDb.Client;
using Absolut.ADDb.Models;


namespace ddb
{
	class MainClass
	{

		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			ADDbClient.Configuration = new ADDbConfiguration
			{
				ApiKey = "c67719d1c318404bbf285837cab887b4",
				ApiSecret = "7f84b6ef3a2e43bb810eff1c57a72e2f",
				PageSize = 10000
			};

			var drinks = ADDbClient.Drinks();


			//Ingredient-># drinks mapping
			System.Collections.Generic.Dictionary<String, int> how_many_drinks = new System.Collections.Generic.Dictionary<String, int>();

			Console.WriteLine("This many drinks: " + drinks.TotalResults);
			int ii = 0;
			foreach (Drink drink in drinks)
			{
				//Console.WriteLine(ii + ": " + drink.Name);
				ii++;


				foreach (DrinkIngredientReference i in drink.Ingredients)
				{
					if (i.Name().Equals("Ice Cubes") ||
						//i.Name().Equals("Lemon Juice") ||
						//i.Name().Equals("Simple Syrup") ||
						//i.Name().Equals("Lime Juice") ||
						i.Name().Equals("Lemon") ||
						//i.Name().Equals("Orange Juice") ||
						i.Name().Equals("Orange") ||
						i.Name().Equals("Crushed Ice") ||
						//i.Name().Equals("Triple Sec") ||
						i.Name().Equals("Lime") ||
						//i.Name().Equals("Maraschino Berry") ||
						//i.Name().Equals("Grenadine") ||
						i.Name().Equals("Soda Water"))
					//i.Name().Equals("Pineapple Juice"))
					{
						continue;
					}

					if (!how_many_drinks.ContainsKey(i.Name()))
					{
						how_many_drinks.Add(i.Name(), 1);
					}
					else
					{
						how_many_drinks[i.Name()]++;
					}
				}
			}

			String[] popular_ingredients = new String[10];
			int[] times_used = new int[10];


			//Now go through and for each possible combination of ten ingredients, see how many drinks can be made entirely with that ten
			String[] essential_ingredients = new String[10];

			String[] all_ingredients = new String[10000];

			int counter = 0;
			foreach (String s in how_many_drinks.Keys)
			{
				all_ingredients[counter++] = s;
			}

			int total_drinks_with_this_set = 0;
			int max_drinks_with_any_set = 0;

			for (int jj = 0; jj < how_many_drinks.Keys.Count - 9; jj++)
			{
				Console.WriteLine("Outer loop: " + jj);
				for (int kk = 1; kk < how_many_drinks.Keys.Count - 8; kk++)
				{
					for (int ll = 2; ll < how_many_drinks.Keys.Count - 7; ll++)
					{
						for (int mm = 3; mm < how_many_drinks.Keys.Count - 6; mm++)
						{
							for (int nn = 4; nn < how_many_drinks.Keys.Count - 5; nn++)
							{
								for (int oo = 5; oo < how_many_drinks.Keys.Count - 4; oo++)
								{
									for (int pp = 6; pp < how_many_drinks.Keys.Count - 3; pp++)
									{
										for (int qq = 7; qq < how_many_drinks.Keys.Count - 2; qq++)
										{
											for (int rr = 8; rr < how_many_drinks.Keys.Count - 1; rr++)
											{
												for (int ss = 9; ss < how_many_drinks.Keys.Count; ss++)
												{
													if (jj == kk || kk == ll || ll == mm || mm == nn || nn == oo || oo == pp || pp == qq || qq == rr || rr == ss)
														continue;

													popular_ingredients[0] = all_ingredients[jj];
													popular_ingredients[1] = all_ingredients[kk];
													popular_ingredients[2] = all_ingredients[ll];
													popular_ingredients[3] = all_ingredients[mm];
													popular_ingredients[4] = all_ingredients[nn];
													popular_ingredients[5] = all_ingredients[oo];
													popular_ingredients[6] = all_ingredients[pp];
													popular_ingredients[7] = all_ingredients[qq];
													popular_ingredients[8] = all_ingredients[rr];
													popular_ingredients[9] = all_ingredients[ss];

													System.Collections.Generic.List<Drink> drinks_we_can_make = new System.Collections.Generic.List<Drink>();

													foreach (Drink d in drinks)
													{
														bool can_make_it = true;
														foreach (var entry in d.Ingredients)
														{
															bool found_ingredient = false;
															for (int aa = 0; aa < 10; aa++)
															{
																if (entry.Name().Equals(essential_ingredients[aa]) || entry.Name().Equals("Ice Cubes") || entry.Name().Equals("Crushed Ice") || entry.Name().Equals("Lemon") || entry.Name().Equals("Orange") || entry.Name().Equals("Lime") || entry.Name().Equals("Soda Water"))
																{
																	found_ingredient = true;
																}
																else
																{
																	//Console.WriteLine("Missing this ingredient: " + entry.Name());
																}
															}
															if (!found_ingredient)
															{
																can_make_it = false;
															}
														}

														if (can_make_it)
														{
															drinks_we_can_make.Add(d);
															total_drinks_with_this_set++;
														}
													}
													if (total_drinks_with_this_set > max_drinks_with_any_set)
													{
														max_drinks_with_any_set = total_drinks_with_this_set;
														essential_ingredients[0] = popular_ingredients[0];
														essential_ingredients[1] = popular_ingredients[1];
														essential_ingredients[2] = popular_ingredients[2];
														essential_ingredients[3] = popular_ingredients[3];
														essential_ingredients[4] = popular_ingredients[4];
														essential_ingredients[5] = popular_ingredients[5];
														essential_ingredients[6] = popular_ingredients[6];
														essential_ingredients[7] = popular_ingredients[7];
														essential_ingredients[8] = popular_ingredients[8];
														essential_ingredients[9] = popular_ingredients[9];
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

			Console.WriteLine("Max number of drinks: " + max_drinks_with_any_set);
			for (int bb = 0; bb < 10; bb++)
			{
				Console.WriteLine(bb + ": " + essential_ingredients[bb]);
			}

#if asdf
			for (ii = 0; ii < 10; ii++)
			{
				//Find the highest, remove it from the dictionary, and store it
				int highest = 0;
				String highest_string = "";
				foreach (var entry in how_many_drinks)
				{
					if (entry.Value > highest)
					{
						highest = entry.Value;
						highest_string = entry.Key;
					}
				}

				how_many_drinks.Remove(highest_string);
				popular_ingredients[ii] = highest_string;
				times_used[ii] = highest;
			}


			for (int index = 0; index < 10; index++)
			{
				Console.WriteLine("Ingredient: " + popular_ingredients[index] + " is used " + times_used[index] + " times");
			}


			drinks = ADDbClient.Drinks()
			                   .With(popular_ingredients[0].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[1].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[2].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[3].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[4].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[5].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[6].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[7].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[8].ToLower().Replace(' ', '-'))
			                   .Or(popular_ingredients[9].ToLower().Replace(' ', '-'))
			                   ;

			System.Collections.Generic.List<Drink> drinks_we_can_make = new System.Collections.Generic.List<Drink>();

			foreach (Drink d in drinks)
			{
				bool can_make_it = true;
				foreach (var entry in d.Ingredients)
				{
					bool found_ingredient = false;
					for (int jj = 0; jj < 10; jj++)
					{
						if (entry.Name().Equals(popular_ingredients[jj]) || entry.Name().Equals("Ice Cubes") || entry.Name().Equals("Crushed Ice") || entry.Name().Equals("Lemon") || entry.Name().Equals("Orange") || entry.Name().Equals("Lime") || entry.Name().Equals("Soda Water"))
						{
							found_ingredient = true;
						}
						else
						{
							//Console.WriteLine("Missing this ingredient: " + entry.Name());
						}
					}
					if (!found_ingredient)
					{
						can_make_it = false;
					}
				}

				if (can_make_it)
				{
					drinks_we_can_make.Add(d);
				}
			}

			Console.WriteLine("We can make this many drinks: " + drinks_we_can_make.Count);
			foreach (Drink d in drinks_we_can_make)
			{
				Console.WriteLine(d.Name);
			}
#endif
		}
	}
}
