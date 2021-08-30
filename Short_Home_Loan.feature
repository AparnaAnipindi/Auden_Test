Feature: Short_Home_Loan
	

Scenario: When selected a weekend date, Scheduled date displays previous friday date
	Given User is at the Home Page
	And Set Slider to 210
	When User clicks a weekend date
	Then Assert that the Schedule date is the friday before the selected sunday


Scenario: The min and max amounts of Loan on slider
	Given User is at the Home Page
	And Set Slider to 210
	When User selects <Loan> value 
	Then Assert that the Slider is moved to <Amount>

	 Examples: 
        | Loan   | Amount |
        | min    | £200	  |
		| max    | £500    |

Scenario: The selected slider amount is Loan amount
	Given User is at the Home Page
	When Slider set to 230
	Then Assert that the slider amount is equal to Loan amount