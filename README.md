# PetDeskAppointmentsAPI
Sample JSON Service + API solution for delivering a report of appointment type/scheduled date frequency lists


PetDesk Appointments API
In this project I will demo a service layer hit to a publicly exposed API at PetDesk to gather a set of Appointments. From this data subset we are concerned with only two datapoints:

-Distribution of Appointment Types : Appointment types range from baths to dental work to surgery. Keep track of the services that are in the highest demand.

-Appointment Requests Received Per Month : Understand the distribution of your clientele. What months have been the busiest for the business?

From these two datapoints I will be gathering and processing frequency of load for each specific item and will be storing
them in the Database. From there, I will provide two API endpoints:

-AppointmentTypeFrequency - to handle appointment types

-AppointmentRequestFrequency - to handle appointment requests

Logging in to my portfolio website at http://khenzel.info:8700 will allow you to create an account within the API which will allow you to authenticate with the API via Bearer
token to request the output from the two respective endpoints. This will produce a report that is intended for view to
the Veterinary Clinic for analysis of business load.
