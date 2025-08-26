 /* Only Alphabets */
        
    function alphab(dec)
         {
           if(window.event.keyCode > 96 && window.event.keyCode < 123)
               {
                  return true;
               }
			   if (window.event.keyCode > 64 && window.event.keyCode < 91)
            {
            return true;
            }
			if (window.event.keyCode ==32)
				{
					return true;
				}
				if (window.event.keyCode ==13)
				{
					return true;
				}
				
			if (window.event.keyCode == 39)
					{
						window.event.keyCode = 0;
						//alert("Apostrophe's Not Allowed");
						return false;
					}
					
               else
                  {
                        window.event.keyCode = 0;
                    alert("Only Alphabets allowed");
                        return false;
               }
               }
               
    /**********************************************************************/
/* Only Alphabets with space for name */
        
     function alphabet(dec)
         {
           if(window.event.keyCode > 96 && window.event.keyCode < 123)
               {
                  return true;
               }
			   if (window.event.keyCode > 64 && window.event.keyCode < 91)
            {
            return true;
            }
			if (window.event.keyCode ==32)
				{
					return true;
				}
				if (window.event.keyCode ==13)
				{
					return true;
				}
				if (window.event.keyCode ==46)
				{
					return true;
				}
			if (window.event.keyCode == 39)
					{
						window.event.keyCode = 0;
						//alert("Apostrophe's Not Allowed");
						return false;
					}
					
               else
                  {
                        window.event.keyCode = 0;
                    alert("Only Alphabets allowed");
                        return false;
               }
               }
               
    /**********************************************************************/
        /* Only Alphabets(Uppercase) with fullstop and space for initial */
        
     function initial(dec)
         {
            if(window.event.keyCode == 46)
              {
                return true;   
              }
            if (window.event.keyCode > 64 && window.event.keyCode < 91)
            {
            return true;
            }  
            if (window.event.keyCode >=97 && window.event.keyCode <= 122)
               {
                return true;
               }
			if (window.event.keyCode ==32)
				{
					return true;
				}
            else
               {
                window.event.keyCode = 0;
				alert("Only Alphabets allowed");
                return false;
               }
               
             
         }
/**********************************************************************
               /* Numbers with fullstop and backspace */

		function numeric( dec )
         {
       
            if (window.event.keyCode > 47 && window.event.keyCode < 58 )
            {
          
            return true;
            }
           
               if(window.event.keyCode == 32 )
               {
              
               window.event.keyCode=0;
               return false;   
               }
               if(window.event.keyCode == 13)
               {
                return true;
               }
               
               else
               {
                  window.event.keyCode = 0;
                  alert("Only Numeric values allowed");
                  return false;
               }
                       
           }
           /**********************************************************************
               /* Numbers with fullstop for amount */

		function amtnumeric( dec )
         {
            if (window.event.keyCode > 47 && window.event.keyCode < 58 )
            {
            return true;
            }
            if (window.event.keyCode ==32)
				{
				    window.event.keyCode = 0;
					return false;
				}
              if (window.event.keyCode ==13)
				{
					return true;
				}
				if (window.event.keyCode ==46)
				{
					return true;
				}
			  else
               {
                  window.event.keyCode = 0;
                  alert("Only Numeric values allowed");
                  return false;
               }
                       
           }
           /**********************************************************************
               /* Numbers with fullstop and backspace */

		function numericdot( dec )
         {
            if (window.event.keyCode > 47 && window.event.keyCode < 58 )
            {
            return true;
            }
               if(window.event.keyCode == 46)
               {
                return true;   
               }
               if(window.event.keyCode == 8)
               {
                return true;
               }
               if (window.event.keyCode == 45) {
                   return true;
               }
               if (window.event.keyCode == 13) {
                   return true;
               }
               else
               {
                  window.event.keyCode = 0;
                  alert("Only Numeric values allowed");
                  return false;
               }
                       
           } 
/*************************************************************************        
         /* Alphabets and Numbers */
         
           function anumeric( dec )
         {
            if (window.event.keyCode >= 48 && window.event.keyCode <= 57 )
            {
            return true;
            }
            if (window.event.keyCode == 35) {
                return true;
            }
            if (window.event.keyCode == 64) {
                return true;
            }
             if (window.event.keyCode >= 65 && window.event.keyCode <= 90 )
            {
            return true;
            }
             if (window.event.keyCode >= 97 && window.event.keyCode <= 122 )
            {
            return true;
             }
			if(window.event.keyCode ==13)
               {
                return true;   
               }
               if(window.event.keyCode ==32)
               {
               window.event.keyCode=0;
                return false;   
               }
               else
               {
                  window.event.keyCode = 0;
                  alert("Only AlphaNumeric values allowed");
                  return false;
               }
           } 
 /*****************************************************************************
             /* Alphabets and Numbers with underscore hiphen  dot space and backspace for address*/
             
           function anumunderscore( dec )
         {
            if(window.event.keyCode == 46)
              {
                return true;   
              }
             if (window.event.keyCode ==32)
				{
					return true;
				}
			if (window.event.keyCode ==8)
				{
					return true;
				}
            if (window.event.keyCode >= 48 && window.event.keyCode <= 57 )
            {
            return true;
            }
             if (window.event.keyCode >= 65 && window.event.keyCode <= 90 )
            {
            return true;
            }
             if (window.event.keyCode >= 97 && window.event.keyCode <= 122 )
            {
            return true;
            }
        if (window.event.keyCode == 95 || window.event.keyCode == 45)
               {
                return true;   
               }
               else
               {
                  window.event.keyCode = 0;
                  alert("Only AlphaNumeric values allowed");
                  return false;
               }
                               

           } 
        
 /******************************************************************************           
            
           /* Upper case alphabets and numbers with space */
               
       function upper_number(dec)
     		{
			if (window.event.keyCode >= 48 && window.event.keyCode <= 57 )
            {
            return true;
            }
           if (window.event.keyCode >= 65 && window.event.keyCode <= 90 )
            {
            return true;
            }
			if( window.event.keyCode == 32)
               {
					return true;
               }           
             
          if (window.event.keyCode >=97 && window.event.keyCode <= 122 )
            {
            window.event.keyCode=window.event.keyCode - 32;
            return true;
            }
            else
            {
                  window.event.keyCode = 0;
                  alert("Only AlphaNumeric values allowed");
                  return false;
               }                             
             }   
               
/*************************************************************************
        /* Uppercase with special characters except apostrope */
        
                  function UpperAllChar(dec)
                    {
                        if (window.event.keyCode==39)
                            {
                            window.event.keyCode = 0;
                            //alert("Apostrophe's Not Allowed");
                            return false;
                            }
                        if (window.event.keyCode >=33 && window.event.keyCode <= 44)
                           {
                            window.event.keyCode = 0;
                            window.event.keyCode=window.event.keyCode - 32;
                            return false;
                           }
                           if (window.event.keyCode >=48 && window.event.keyCode <= 57)
                            {
                            //window.event.keyCode=window.event.keyCode - 32;
                            window.event.keyCode = 0;
                            return false;
                            }
                            if (window.event.keyCode >=97 && window.event.keyCode <= 122)
                            {
                            window.event.keyCode=window.event.keyCode - 32;
                            return true;
                            }
                         else
                            {
                            return true;
                            }
                    } 
     /***************************************************************************
             /* Lower Case Alphabets with special Characters */
             
                 function alpha(dec)
			       {
					    if (window.event.keyCode==39)
					    {
						    window.event.keyCode = 0;
						    alert("Apostrophe's Not Allowed");
						    return false;
					    }
					    if (window.event.keyCode == 46 )
                        {                            
                            return true;
                        }
					    if (window.event.keyCode >=33 && window.event.keyCode <= 47 )
                        {
                            window.event.keyCode = 0;
                            //window.event.keyCode=window.event.keyCode - 32;
                            return false;
                        }
                        if (window.event.keyCode >=91 && window.event.keyCode <= 96 )
                        {
                            window.event.keyCode = 0;
                            //window.event.keyCode=window.event.keyCode - 32;
                            return false;
                        }
					    if (window.event.keyCode >=97 && window.event.keyCode <= 122 )
                        {
                            //window.event.keyCode=window.event.keyCode - 32;
                            return true;
                        }
                        if (window.event.keyCode == 46 )
                        {                            
                            return true;
                        }
					    else
					    {
						    return true;
					    }
				}
/******************************************************************************
	             /* Only Numbers */
	             
          function integer(dec)
           {
            if (window.event.keyCode > 47 && window.event.keyCode < 58 )
            {
            return true;
            }
             if(window.event.keyCode == 13)
               {
                return true;
               }  
               if(window.event.keyCode == 8)
               {
                return true;
               }
               
               else
               {
                  window.event.keyCode = 0;
                  alert("Only integer values allowed");
                  return false;
               }
                                   
           } 
 /***************************************************************************
                 /* Alphabets and Numbers without Apostrophe */
                 
         function rem(dec)
			   {
					if (window.event.keyCode==39)
					{
						window.event.keyCode = 0;						
						return false;
					}
					else
					{
						return true;
					}
				}
/*************************************************************************
             /* Both case Alphabets with comma,space and fullstop */
             
         function alias(dec)
         {
            if (window.event.keyCode > 64 && window.event.keyCode < 91)
            {
            return true;
            }
               if(window.event.keyCode > 96 && window.event.keyCode < 123)
               {
                  return true;
               }
                if( window.event.keyCode == 46)
               {
					return true;
               }
                if( window.event.keyCode == 32)
               {
					return true;
               }  
                if( window.event.keyCode == 44)
               {
					return true;
               }  
                  else
                  {
                        window.event.keyCode = 0;
                    alert("Only Alphabets allowed");
                        return;
               }
          }     
/***************************************************************************
                  /* Numbers with hiphen */
                  
               function phone(dec)
                {
                    if (window.event.keyCode > 47 && window.event.keyCode < 58 )
                    {
                    return true;
                    }
 
                    if(window.event.keyCode == 8)
                    {
                    return true;
                    }
					if(window.event.keyCode == 13)
                    {
                    return true;
                    }
                    if(window.event.keyCode == 45)
                    {
                    return true;
                    }
                    else
                    {
                    window.event.keyCode = 0;
                    alert("Only numbers allowed");
                    return false;
                    }
                } 
/****************************************************************************** 
      /* Lower case Alphabets with comma,space and fullstop */
      
			function alphacomma( dec )
			{
				if(window.event.keyCode > 96 && window.event.keyCode < 123)
					{
						return true;
					}
				if( window.event.keyCode == 44)
					{
						return true;
					}
				if( window.event.keyCode == 46)
					{
						return true;
					}
				if(window.event.keyCode == 13)
                    {
                    return true;
                    }
				if( window.event.keyCode == 32)
					{
					return true;
					} 
					else
					{
					window.event.keyCode = 0;
					alert("Only Alphabets allowed");
						return;
					}
					}
/*************************************************************************/                             
            /* Alphabets and Numbers with underscore hiphen */
             
           function anumunderscorehpn( dec )
         {
            if (window.event.keyCode >= 48 && window.event.keyCode <= 57 )
            {
            return true;
            }
             if (window.event.keyCode >= 65 && window.event.keyCode <= 90 )
            {
            return true;
            }
             if (window.event.keyCode >= 97 && window.event.keyCode <= 122 )
            {
            return true;
            }
            if(window.event.keyCode == 95 || window.event.keyCode == 45)
               {
                return true;   
               }
               else
               {
                  window.event.keyCode = 0;
                  alert("Only AlphaNumeric values allowed");
                  return false;
               }
                               

           } 
        
/**********************************************************************
        /* Only Alphabets(Uppercase)  and space for initial */
        
     function alphaspac(dec)
         {
            if (window.event.keyCode > 64 && window.event.keyCode < 91)
            {
            return true;
            }  
            if (window.event.keyCode >=97 && window.event.keyCode <= 122)
               {
                window.event.keyCode=window.event.keyCode - 32;
                return true;
               }
			   if (window.event.keyCode==32)
               {
                 return true;
               }
            else
               {
                window.event.keyCode = 0;
				alert("Only Alphabets allowed");
                return false;
               }
               
             
         }
/**********************************************************************/
/* Only Alphabets with space for address */
        
     function alphabetaddress(dec)
         {
           if(window.event.keyCode > 96 && window.event.keyCode < 123)
               {
                  return true;
               }
			   if (window.event.keyCode > 64 && window.event.keyCode < 91)
            {
            return true;
            }
			if (window.event.keyCode > 47 && window.event.keyCode < 58 )
                    {
                    return true;
                    }
			if (window.event.keyCode ==13)
				{
					return true;
				}
				if (window.event.keyCode ==45)
				{
					return true;
				}
				if (window.event.keyCode ==47)
				{
					return true;
				}
				if (window.event.keyCode ==58)
				{
					return true;
				}
				if (window.event.keyCode ==59)
				{
					return true;
				}
				if (window.event.keyCode ==32)
				{
					return true;
				}
				if (window.event.keyCode ==35)
				{
					return true;
				}
				if (window.event.keyCode ==44)
				{
					return true;
				}
				if (window.event.keyCode ==46)
				{
					return true;
				}
			if (window.event.keyCode == 39)
					{
						window.event.keyCode = 0;
						//alert("Apostrophe's Not Allowed");
						return false;
					}
					
               else
                  {
                        window.event.keyCode = 0;
                    alert("Only Alphanumerics allowed");
                        return false;
               }
               }
               
    /**********************************************************************/
                     /*For date Field*/
 function integerdate(dec)
           {
           
            if (window.event.keyCode > 47 && window.event.keyCode < 58 )
            {
            return true;
            }
             if(window.event.keyCode == 13)
               {
                return true;
               }
             if(window.event.keyCode == 45)
               {
                return true;
               }
			   if(window.event.keyCode== 47)
			   {
				   return true;
			   }
               if(window.event.keyCode == 8)
               {
                return true;
               }
               
               else
               {
                  window.event.keyCode = 0;
                  alert("Only integer values allowed");
                  return false;
               }
                                   
           } 
 /***************************************************************************/
           function padZero(curval, mantissa, decimals) {
               if (curval == 'undefined') { curval = 0; }
               if (curval.toString() == "") { return; }
               // debugger


               if (decimals == 'undefined' || decimals == "") { decimals = 0; }
               if (mantissa == 'undefined' || mantissa == "") { mantissa = 0; }
               // debugger
               var tempstr = curval.toString();
               var retstr;
               var zeroString = '0';
               if (tempstr.indexOf(".") < 0) {
                   retstr = tempstr + '.0';

               } else { retstr = curval.toString(); }

               var itm = retstr.split(".");

               itm[0] = padleft(itm[0], mantissa);
               if (itm[0] == '') { return; }
               itm[1] = padright(itm[1], decimals);
               if (itm[1] == '') { return; }
               if (itm.length > 2) { retstr = itm[0].toString() + '.' + itm[1].toString(); }
               else { retstr = itm.join("."); }
               return retstr.toString();

           }
           function padleft(number, leng) {
               var neg;
               var firstChar = number.substring(0, 1);
               if (firstChar == '-') {
                   neg = true;
                   number = number.substring(1);
               } else {
                   neg = false;
               }
               var str = '' + number;

               while (str.length < leng) {
                   str = '0' + str;
               }
               if (str.indexOf("-") >= 0) {
                   str = str.replace(/-/gi, "")
               }

               if (str.length > leng && leng > 0) {
                   // str = str.substring(0, leng);
                   alert('Mantissa is Less than Entered Value');
                   str = '';
                   return str;
               }
               if (neg == true) { str = '-' + str; }
               return str;

           }
           function padright(number, leng) {
               var firstChar = number.substring(0, 1);
               if (firstChar == '-') {
                   number = number.substring(1);
               }
               var str = '' + number;
               if (str.indexOf("-") >= 0) {
                   str = str.replace(/-/gi, "")
               }
               while (str.length < leng) {
                   str = str + '0';
               }
               if (str.length > leng && leng > 0) {
                   alert('decimal is less than entered value');
                   //    str = str.substring(0, leng);
                   str = '';
                   return str;
               }
               return str;

           }

         