This is a consol program in C# to preview hotel room availability and reservations. 

The application reads from .json files containing hotel data and booking data, then allow a user to check room availability for a specified hotel, date range, and room type. 

Hotels sometimes accept over bookings so the value can be negative to indicate this.(For example :-1)

The program ends when a blank line is entered.  


**********************HOW TO USE*********************

To run the program, write the example code to the terminal : .\BookingApp_Test.exe --hotels hotels.json --bookings bookings.json

For the query in the "Availability(Hotel_ID,StartDate-EndDate(yyyymmdd),Hotel Type(For single SGL or for Double DBL ))" format entered into the program, the program gives the number of availability for the specified room type and the specified date range as output.
As an example you can try the below: 
*Availability(H1, 20240901, SGL)
*Availability(H1, 20240901-20240903, DBL)
*Availability(H1,20240901-20240903,SGL)
*Availability(H1,20240908-20240913,SGL)

*** While coding the program, it was assumed that the user already knows the Hotel IDs.
    You can access this data from the hotels.json file.
    You can also access the reservation data from bookings.json file to check if the result is correct.


                


