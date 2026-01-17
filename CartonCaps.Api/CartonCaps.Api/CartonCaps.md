CartonCaps API – Referral Feature

Base URL
https://api.cartoncaps.com/api/referrals

Create a new referral

	Endpoint:			POST /api/referrals
	Description:		Generates a new code for an existing user.
	Query parameters:	userId-> string and required ID of the user generating the referral

	Request example:
	POST /api/referrals?userId=user_001
	Response 201 Created:
	{
	  "referralId": 7,
	  "code": "REF10007",
	  "userId": "user_001",
	  "status": "pending",
	  "expiration": "2026-03-01T00:00:00Z",
	  "redeemDate": null,
	  "referredUserId": null
	}

	Possible errors: 
		 404:User not found
		 500:Internal server error

Get all referrals created by user

	Endpoint:			GET /api/referrals
	Description:		Retrieves all referrals created by a specific user.
	Query parameters:	userId-> string and required ID of the user

	Request example:
	GET /api/referrals?userId=user_001
	Response 200 OK:
	[
	  {
		"referralId": 1,
		"code": "REF10001",
		"userId": "user_001",
		"status": "pending",
		"expiration": "2026-03-15T00:00:00Z",
		"redeemDate": null,
		"referredUserId": null
	  },
	  {
		"referralId": 2,
		"code": "REF10002",
		"userId": "user_001",
		"status": "redeemed",
		"expiration": "2026-02-01T00:00:00Z",
		"redeemDate": "2026-01-10T14:30:00Z",
		"referredUserId": "user_002"
	  }
	]

	Possible errors:
	404: User not found 
	500: Internal server error 


	Notes:
		Status: pending, redeemed, expired. 

Redeem a referral

	Endpoint:			POST /api/referrals/redeem
	Description:		Redeems an existing code for a new user.
	Query parameters:  code-> string and required, code to redeem referred
						userId-> string and required,ID of the new user redeeming the referral.

	Request example:
	POST /api/referrals/redeem?code=REF10001&amp;referredUserId=user_005

	Response 200 OK:
	{
	  "referralId": 1,
	  "code": "REF10001",
	  "userId": "user_001",
	  "status": "redeemed",
	  "expiration": "2026-03-15T00:00:00Z",
	  "redeemDate": "2026-01-15T12:00:00Z",
	  "referredUserId": "user_005"
	}

	Possible errors:
	400: Referral is invalid, already redeemed, or the user is the same as the referrer  
	404: Referral code not found                                            
	500: Internal server error     


Get referral by code

	Endpoint:		 GET /api/referrals/{code}
	Description:	 Retrieves details of a specific referral by its code.
	Path parameters: code-> string and required,referral code

	Request example:
	GET /api/referrals/REF10001

	Response 200 OK:
	{
	  "referralId": 1,
	  "code": "REF10001",
	  "userId": "user_001",
	  "status": "redeemed",
	  "expiration": "2026-03-15T00:00:00Z",
	  "redeemDate": "2026-01-15T12:00:00Z",
	  "referredUserId": "user_005"
	}

	Possible errors:
	404: Referral code not found 
	500: Internal server error   


**Considerations 

	Reward and abuse prevention:
	Ensure the same user cannot redeem their own referral.

	Check for duplicate referrals and implement limits per user if needed.

	In a bigger project, the folders Common,  Model, Repository and Service, will be placed in class libraries.

	In this case, the file data.json, is used as "storage data base", with dummy data.