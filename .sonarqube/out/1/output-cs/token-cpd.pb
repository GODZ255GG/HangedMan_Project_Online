á
\C:\Users\ReneU\Documents\Github\HangedMan_Project_Online\Services\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str #
)# $
]$ %
[ 
assembly 	
:	 

AssemblyDescription 
( 
$str !
)! "
]" #
[		 
assembly		 	
:			 
!
AssemblyConfiguration		  
(		  !
$str		! #
)		# $
]		$ %
[

 
assembly

 	
:

	 

AssemblyCompany

 
(

 
$str

 
)

 
]

 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str %
)% &
]& '
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[ 
assembly 	
:	 

AssemblyVersion 
( 
$str $
)$ %
]% &
[   
assembly   	
:  	 

AssemblyFileVersion   
(   
$str   (
)  ( )
]  ) *Ú
RC:\Users\ReneU\Documents\Github\HangedMan_Project_Online\Services\IWordServices.cs
	namespace 	
Services
 
{ 
[ 
ServiceContract 
] 
public 

	interface 
IWordServices "
{		 
[

 	
OperationContract

	 
]

 
List 
< 
Category 
> 
GetCategories $
($ %
)% &
;& '
[ 	
OperationContract	 
] 
List 
< 
Word 
> 
GetWordsPerCategory &
(& '
int' *
category+ 3
)3 4
;4 5
[ 	
OperationContract	 
] 
string 
GetWordSpanish 
( 
int !
wordID" (
)( )
;) *
[ 	
OperationContract	 
] 
string 
GetWordEnglish 
( 
int !
wordID" (
)( )
;) *
[ 	
OperationContract	 
] 
string 
GetClueSpanish 
( 
int !
wordID" (
)( )
;) *
[ 	
OperationContract	 
] 
string 
GetClueEnglish 
( 
int !
wordID" (
)( )
;) *
[ 	
OperationContract	 
] 
string 
GetCategoryByWordID "
(" #
int# &
wordID' -
,- .
int/ 2
matchLanguage3 @
)@ A
;A B
} 
} “
TC:\Users\ReneU\Documents\Github\HangedMan_Project_Online\Services\IPlayerServices.cs
	namespace 	
Services
 
{ 
[ 
ServiceContract 
] 
public 

	interface 
IPlayerServices $
{ 
[		 	
OperationContract			 
]		 
bool

 
RegisterPlayer

 
(

 
Player

 "
	newPlayer

# ,
)

, -
;

- .
[ 	
OperationContract	 
] 
bool "
EmailAlreadyRegistered #
(# $
string$ *
email+ 0
)0 1
;1 2
[ 	
OperationContract	 
] 
bool %
NicknameAlreadyRegistered &
(& '
string' -
nickname. 6
)6 7
;7 8
[ 	
OperationContract	 
] 
bool !
TelephoneAlreadyExist "
(" #
string# )
	telephone* 3
)3 4
;4 5
[ 	
OperationContract	 
] 
Player 
LogIn 
( 
string 
email !
,! "
string# )
password* 2
)2 3
;3 4
[ 	
OperationContract	 
] 
bool 
UpdatePlayerProfile  
(  !
Player! '
updatePlayer( 4
)4 5
;5 6
[ 	
OperationContract	 
] 
int 
	GetPoints 
( 
int 
playerID "
)" #
;# $
} 
} Û
RC:\Users\ReneU\Documents\Github\HangedMan_Project_Online\Services\IGameServices.cs
	namespace 	
Services
 
{ 
[ 
ServiceContract 
] 
public 

	interface 
IGameServices "
{		 
[

 	
OperationContract

	 
]

 
Match 
CreateMatch 
( 
Match 
createMatch  +
)+ ,
;, -
[ 	
OperationContract	 
] 
List 
< 
Match 
> 
GetMatchList  
(  !
int! $
playerID% -
)- .
;. /
[ 	
OperationContract	 
] 
List 
< 
Match 
> 
GetMatchesPlayed $
($ %
int% (
playerID) 1
)1 2
;2 3
[ 	
OperationContract	 
] 
bool 
	InitMatch 
( 
int 
guestID "
," #
int$ '
matchID( /
)/ 0
;0 1
[ 	
OperationContract	 
] 
string 
GetGuestNickName 
(  
int  #
playerID$ ,
), -
;- .
[ 	
OperationContract	 
] 
bool 
IsThereGuest 
( 
int 
matchID %
)% &
;& '
[ 	
OperationContract	 
] 
bool 

LeaveMatch 
( 
int 
matchID #
)# $
;$ %
[ 	
OperationContract	 
] 
char 
? 
GetGuestLetter 
( 
int  
matchID! (
)( )
;) *
[ 	
OperationContract	 
] 
int  
GetRemainingAttempts  
(  !
int! $
matchID% ,
), -
;- .
[ 	
OperationContract	 
] 
void 
UpdatePointsEarned 
(  
int  #
matchID$ +
,+ ,
int- 0
playerID1 9
)9 :
;: ;
[ 	
OperationContract	 
] 
void 
PenalizeAbandon 
( 
int  
playerID! )
)) *
;* +
[   	
OperationContract  	 
]   
bool!! 
FinishMatch!! 
(!! 
int!! 
matchID!! $
)!!$ %
;!!% &
["" 	
OperationContract""	 
]"" 
bool## 
UpdateCharBD## 
(## 
char## 
letter## %
,##% &
int##' *
matchID##+ 2
)##2 3
;##3 4
[$$ 	
OperationContract$$	 
]$$ 
int%% 
GetMatchStatus%% 
(%% 
int%% 
matchID%% &
)%%& '
;%%' (
[&& 	
OperationContract&&	 
]&& 
bool'' #
UpdateRemainingAttempts'' $
(''$ %
int''% (
remainingAttempts'') :
,'': ;
int''< ?
matchID''@ G
)''G H
;''H I
[(( 	
OperationContract((	 
](( 
bool)) 
UpdateWinner)) 
()) 
int)) 
playerID)) &
,))& '
int))( +
matchID)), 3
)))3 4
;))4 5
[** 	
OperationContract**	 
]** 
int++ 
?++ 
GetWinnerID++ 
(++ 
int++ 
matchID++ $
)++$ %
;++% &
},, 
}-- ›Z
UC:\Users\ReneU\Documents\Github\HangedMan_Project_Online\Services\HangedManService.cs
	namespace 	
Services
 
{ 
public 

partial 
class 
HangedManService )
:* +
IPlayerServices, ;
{ 
public		 
bool		 
RegisterPlayer		 "
(		" #
Player		# )
	newPlayer		* 3
)		3 4
{

 	
return 
	PlayerDTO 
. 
RegisterPlayer +
(+ ,
	newPlayer, 5
)5 6
;6 7
} 	
public 
bool "
EmailAlreadyRegistered *
(* +
string+ 1
email2 7
)7 8
{ 	
return 
	PlayerDTO 
. "
EmailAlreadyRegistered 3
(3 4
email4 9
)9 :
;: ;
} 	
public 
bool %
NicknameAlreadyRegistered -
(- .
string. 4
nickname5 =
)= >
{ 	
return 
	PlayerDTO 
. %
NicknameAlreadyRegistered 6
(6 7
nickname7 ?
)? @
;@ A
} 	
public 
bool !
TelephoneAlreadyExist )
() *
string* 0
	telephone1 :
): ;
{ 	
return 
	PlayerDTO 
. &
TelephoneAlreadyRegistered 7
(7 8
	telephone8 A
)A B
;B C
} 	
public 
Player 
LogIn 
( 
string "
email# (
,( )
string* 0
password1 9
)9 :
{ 	
return 
	PlayerDTO 
. 
LogIn "
(" #
email# (
,( )
password* 2
)2 3
;3 4
}   	
public"" 
bool"" 
UpdatePlayerProfile"" '
(""' (
Player""( .
updatePlayer""/ ;
)""; <
{## 	
return$$ 
	PlayerDTO$$ 
.$$ 
UpdatePlayerProfile$$ 0
($$0 1
updatePlayer$$1 =
)$$= >
;$$> ?
}%% 	
public'' 
int'' 
	GetPoints'' 
('' 
int''  
playerID''! )
)'') *
{(( 	
return)) 
	PlayerDTO)) 
.)) 
	GetPoints)) &
())& '
playerID))' /
)))/ 0
;))0 1
}** 	
}++ 
public-- 

partial-- 
class-- 
HangedManService-- )
:--* +
IGameServices--, 9
{.. 
public// 
Match// 
CreateMatch//  
(//  !
Match//! &
createMatch//' 2
)//2 3
{00 	
return11 
MatchDTO11 
.11 
CreateMatch11 '
(11' (
createMatch11( 3
)113 4
;114 5
}22 	
public44 
List44 
<44 
Match44 
>44 
GetMatchesPlayed44 +
(44+ ,
int44, /
playerID440 8
)448 9
{55 	
return66 
MatchDTO66 
.66 
GetMatchesPlayed66 ,
(66, -
playerID66- 5
)665 6
;666 7
}77 	
public99 
List99 
<99 
Match99 
>99 
GetMatchList99 '
(99' (
int99( +
playerID99, 4
)994 5
{:: 	
return;; 
MatchDTO;; 
.;;  
GetMatchesAvaliables;; 0
(;;0 1
playerID;;1 9
);;9 :
;;;: ;
}<< 	
public>> 
bool>> 
	InitMatch>> 
(>> 
int>> !
guestID>>" )
,>>) *
int>>+ .
matchID>>/ 6
)>>6 7
{?? 	
return@@ 
MatchDTO@@ 
.@@ 
	InitMatch@@ %
(@@% &
guestID@@& -
,@@- .
matchID@@/ 6
)@@6 7
;@@7 8
}AA 	
publicCC 
stringCC 
GetGuestNickNameCC &
(CC& '
intCC' *
playerIDCC+ 3
)CC3 4
{DD 	
returnEE 
MatchDTOEE 
.EE 
GetGuestNickNameEE ,
(EE, -
playerIDEE- 5
)EE5 6
;EE6 7
}FF 	
publicHH 
boolHH 
IsThereGuestHH  
(HH  !
intHH! $
matchIDHH% ,
)HH, -
{II 	
returnJJ 
MatchDTOJJ 
.JJ 
IsThereGuestJJ (
(JJ( )
matchIDJJ) 0
)JJ0 1
;JJ1 2
}KK 	
publicMM 
boolMM 

LeaveMatchMM 
(MM 
intMM "
matchIDMM# *
)MM* +
{NN 	
returnOO 
MatchDTOOO 
.OO 

LeaveMatchOO &
(OO& '
matchIDOO' .
)OO. /
;OO/ 0
}PP 	
publicRR 
boolRR 
FinishMatchRR 
(RR  
intRR  #
matchIDRR$ +
)RR+ ,
{SS 	
returnTT 
MatchDTOTT 
.TT 
FinishMatchTT '
(TT' (
matchIDTT( /
)TT/ 0
;TT0 1
}UU 	
publicWW 
charWW 
?WW 
GetGuestLetterWW #
(WW# $
intWW$ '
matchIDWW( /
)WW/ 0
{XX 	
returnYY 
MatchDTOYY 
.YY 
GetGuestLetterYY *
(YY* +
matchIDYY+ 2
)YY2 3
;YY3 4
}ZZ 	
public\\ 
int\\ 
GetMatchStatus\\ !
(\\! "
int\\" %
matchID\\& -
)\\- .
{]] 	
return^^ 
MatchDTO^^ 
.^^ 
GetMatchStatus^^ *
(^^* +
matchID^^+ 2
)^^2 3
;^^3 4
}__ 	
publicaa 
intaa  
GetRemainingAttemptsaa '
(aa' (
intaa( +
matchIDaa, 3
)aa3 4
{bb 	
returncc 
MatchDTOcc 
.cc  
GetRemainingAttemptscc 0
(cc0 1
matchIDcc1 8
)cc8 9
;cc9 :
}dd 	
publicff 
boolff 
UpdateCharBDff  
(ff  !
charff! %
letterff& ,
,ff, -
intff. 1
matchIDff2 9
)ff9 :
{gg 	
returnhh 
MatchDTOhh 
.hh 
UpdateCharBDhh (
(hh( )
letterhh) /
,hh/ 0
matchIDhh1 8
)hh8 9
;hh9 :
}ii 	
publickk 
voidkk 
UpdatePointsEarnedkk &
(kk& '
intkk' *
matchIDkk+ 2
,kk2 3
intkk4 7
playerIDkk8 @
)kk@ A
{ll 	
MatchDTOmm 
.mm 
UpdatePointsEarnedmm '
(mm' (
matchIDmm( /
,mm/ 0
playerIDmm1 9
)mm9 :
;mm: ;
}nn 	
publicpp 
voidpp 
PenalizeAbandonpp #
(pp# $
intpp$ '
playerIDpp( 0
)pp0 1
{qq 	
MatchDTOrr 
.rr 
PenalizeAbandonrr $
(rr$ %
playerIDrr% -
)rr- .
;rr. /
}ss 	
publicuu 
booluu #
UpdateRemainingAttemptsuu +
(uu+ ,
intuu, /
remainingAttemptsuu0 A
,uuA B
intuuC F
matchIDuuG N
)uuN O
{vv 	
returnww 
MatchDTOww 
.ww #
UpdateRemainingAttemptsww 3
(ww3 4
remainingAttemptsww4 E
,wwE F
matchIDwwG N
)wwN O
;wwO P
}xx 	
publiczz 
boolzz 
UpdateWinnerzz  
(zz  !
intzz! $
playerIDzz% -
,zz- .
intzz/ 2
matchIDzz3 :
)zz: ;
{{{ 	
return|| 
MatchDTO|| 
.|| 
UpdateWinner|| (
(||( )
playerID||) 1
,||1 2
matchID||3 :
)||: ;
;||; <
}}} 	
public 
int 
? 
GetWinnerID 
(  
int  #
matchID$ +
)+ ,
{
ÄÄ 	
return
ÅÅ 
MatchDTO
ÅÅ 
.
ÅÅ 
GetWinnerID
ÅÅ '
(
ÅÅ' (
matchID
ÅÅ( /
)
ÅÅ/ 0
;
ÅÅ0 1
}
ÇÇ 	
}
ÉÉ 
public
ÖÖ 

partial
ÖÖ 
class
ÖÖ 
HangedManService
ÖÖ )
:
ÖÖ* +
IWordServices
ÖÖ, 9
{
ÜÜ 
public
áá 
List
áá 
<
áá 
Category
áá 
>
áá 
GetCategories
áá +
(
áá+ ,
)
áá, -
{
àà 	
return
ââ 
CategoryDTO
ââ 
.
ââ 
GetCategories
ââ ,
(
ââ, -
)
ââ- .
;
ââ. /
}
ää 	
public
åå 
string
åå 
GetClueEnglish
åå $
(
åå$ %
int
åå% (
wordID
åå) /
)
åå/ 0
{
çç 	
return
éé 
WordDTO
éé 
.
éé 
GetClueEnglish
éé )
(
éé) *
wordID
éé* 0
)
éé0 1
;
éé1 2
}
èè 	
public
ëë 
string
ëë 
GetClueSpanish
ëë $
(
ëë$ %
int
ëë% (
wordID
ëë) /
)
ëë/ 0
{
íí 	
return
ìì 
WordDTO
ìì 
.
ìì 
GetClueSpanish
ìì )
(
ìì) *
wordID
ìì* 0
)
ìì0 1
;
ìì1 2
}
îî 	
public
ññ 
string
ññ 
GetWordEnglish
ññ $
(
ññ$ %
int
ññ% (
wordID
ññ) /
)
ññ/ 0
{
óó 	
return
òò 
WordDTO
òò 
.
òò 
GetWordEnglish
òò )
(
òò) *
wordID
òò* 0
)
òò0 1
;
òò1 2
}
ôô 	
public
õõ 
string
õõ 
GetWordSpanish
õõ $
(
õõ$ %
int
õõ% (
wordID
õõ) /
)
õõ/ 0
{
úú 	
return
ùù 
WordDTO
ùù 
.
ùù 
GetWordSpanish
ùù )
(
ùù) *
wordID
ùù* 0
)
ùù0 1
;
ùù1 2
}
ûû 	
public
†† 
List
†† 
<
†† 
Word
†† 
>
†† !
GetWordsPerCategory
†† -
(
††- .
int
††. 1
category
††2 :
)
††: ;
{
°° 	
return
¢¢ 
WordDTO
¢¢ 
.
¢¢ !
GetWordsPerCategory
¢¢ .
(
¢¢. /
category
¢¢/ 7
)
¢¢7 8
;
¢¢8 9
}
££ 	
public
•• 
string
•• !
GetCategoryByWordID
•• )
(
••) *
int
••* -
wordID
••. 4
,
••4 5
int
••6 9
matchLanguage
••: G
)
••G H
{
¶¶ 	
return
ßß 
WordDTO
ßß 
.
ßß !
GetCategoryByWordID
ßß .
(
ßß. /
wordID
ßß/ 5
,
ßß5 6
matchLanguage
ßß7 D
)
ßßD E
;
ßßE F
}
®® 	
}
™™ 
}´´ 