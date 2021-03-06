
# Hackathon Submission Entry form

## Team name

Inspire

## Category

The best enhancement to the Sitecore Admin (XP) for Content Editors & Marketers

## Description
 - Module Name: Cloud Connector

 - Module Purpose
This module will connect Sitecore centrally with any of the cloud providers like Azure, AWS, Google and provide flexibility to marketers and content editors to save Sitecore data to cloud from any of the Sitecore marketing apps like Sitecore Experience Forms, Sitecore marketing Automation or personalization data etc. In our current implementation, we are saving contact's data to Azure storage from Sitecore forms and marketing automation and then using azure function, we are sending SMS notification to those contacts. 

 - What problem was solved
Sitecore provided basic out of the box functionalities/actions like send email to contacts. But let's say, marketers wants to engage customers with SMS, WhatsApp or through any other way.
And here cloud connector come into the pictures with above problem's solutions.
So instead of creating custom functionality for each of these actions in Sitecore, we are utilizing cloud computing to perform these actions.
This way we are not overloading Sitecore with the tasks that can be performed outside of Sitecore and performance of Sitecore will not be hampered.

 -   How does this module solve it



 - How to use this:

Go to Sitecore launchpad -> click on Cloud Connector app.

On the cloud connector page, first choose the cloud provider. **This implementation is working for Azure cloud provider.**

Enter the Azure storage account credentials and click on submit.

If credentials are valid, then Sitecore will be connected to Azure storage account. 

Now you can use with Sitecore forms or Sitecore marketing automation 
application to save contacts data in Azure.

First let's see how to use this module in Sitecore Forms as below screenshots

![SendToAzure Custom Action button on Form](docs/images/SendToAzure-CustomButton.PNG?raw=true "Azure Custom Action")

![Contact us page View](docs/images/contact-us-page.PNG?raw=true "Contact Us Page")

## Video link

[CloudConnector Video Url](#https://www.youtube.com/watch?v=Il6v5sHQ)


## Pre-requisites and Dependencies

- Dependencies
Sitecore 10.1.0 rev. 005207 (WDP XP0 packages).zip
Microsoft Azure
Content editor & marketers must need azure Credentials


## Installation instructions

Install Sitecore 10.1.0 rev. 005207 [Sitecore Experience Platform 10.1](https://dev.sitecore.net/Downloads/Sitecore_Experience_Platform/101/Sitecore_Experience_Platform_101.aspx)
Install [cloud connector module](https://github.com/Sitecore-Hackathon/2021-InSpire/raw/develop-pk/docs/cloud%20connector.zip)
Smart Publish


### Configuration


## Usage instructions
⟹ Provide documentation about your module, how do the users use your module, where are things located, what do the icons mean, are there any secret shortcuts etc.

Include screenshots where necessary. You can add images to the `./images` folder and then link to them from your documentation:

![Hackathon Logo](docs/images/hackathon.png?raw=true "Hackathon Logo")

You can embed images of different formats too:

![Deal With It](docs/images/deal-with-it.gif?raw=true "Deal With It")


## Comments

