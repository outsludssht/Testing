Feature: EHU Website Functionality
  As a user of the EHU website
  I want to be able to navigate the site, search for content, and access contact information
  So that I can find information about the university

Scenario: Navigate to About page
  Given I am on the EHU main page
  When I click on the About link
  Then I should be redirected to the About page
  And the page header should display "About"

Scenario Outline: Search for content on the website
  Given I am on the EHU main page
  When I search for "<query>"
  Then I should be redirected to the search results page with URL "<expectedUrl>"

  Examples:
    | query          | expectedUrl                          |
    | study programs | https://en.ehu.lt/?s=study+programs  |
    | faculty        | https://en.ehu.lt/?s=faculty         |

Scenario: Switch language to Lithuanian
  Given I am on the EHU main page
  When I switch the language to Lithuanian
  Then I should be redirected to the Lithuanian version of the website

Scenario: Verify contact information
  Given I am on the EHU contact page
  Then the email information should be "E-mail: franciskscarynacr@gmail.com"
  And the Lithuanian phone number should be "Phone (LT): +370 68 771365"
  And the Belarusian phone number should be "Phone (BY): +375 29 5781488"
  And the social media text should include Facebook, Telegram and VK 