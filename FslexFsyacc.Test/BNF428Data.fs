﻿namespace FslexFsyacc

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.YACCs
open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.Precedences

/// 文法 4.28 推导的数据集合
module BNF428Data =
    let inputProductionList = [["E";"T";"E'"];["E'";"+";"T";"E'"];["E'"];["T";"F";"T'"];["T'";"*";"F";"T'"];["T'"];["F";"(";"E";")"];["F";"id"]]
    let mainProductions = set [["E";"T";"E'"];["E'"];["E'";"+";"T";"E'"];["F";"(";"E";")"];["F";"id"];["T";"F";"T'"];["T'"];["T'";"*";"F";"T'"]]
    let augmentedProductions = set [["";"E"];["E";"T";"E'"];["E'"];["E'";"+";"T";"E'"];["F";"(";"E";")"];["F";"id"];["T";"F";"T'"];["T'"];["T'";"*";"F";"T'"]]
    let symbols = set ["(";")";"*";"+";"E";"E'";"F";"T";"T'";"id"]
    let nonterminals = set ["E";"E'";"F";"T";"T'"]
    let terminals = set ["(";")";"*";"+";"id"]
    let nullables = set ["E'";"T'"]
    let firsts = Map ["E",set ["(";"id"];"E'",set ["+"];"F",set ["(";"id"];"T",set ["(";"id"];"T'",set ["*"]]
    let lasts = Map ["E",set [")";"id"];"E'",set [")";"id"];"F",set [")";"id"];"T",set [")";"id"];"T'",set [")";"id"]]
    let follows = Map ["(",set ["(";"id"];")",set ["";")";"*";"+"];"*",set ["(";"id"];"+",set ["(";"id"];"E",set ["";")"];"E'",set ["";")"];"F",set ["";")";"*";"+"];"T",set ["";")";"+"];"T'",set ["";")";"+"];"id",set ["";")";"*";"+"]]
    let precedes = Map ["(",set ["";"(";"*";"+"];")",set [")";"id"];"*",set [")";"id"];"+",set [")";"id"];"E",set ["";"("];"E'",set [")";"id"];"F",set ["";"(";"*";"+"];"T",set ["";"(";"+"];"T'",set [")";"id"];"id",set ["";"(";"*";"+"]]
    let itemCores = set [{production= ["";"E"];dot= 0};{production= ["";"E"];dot= 1};{production= ["E";"T";"E'"];dot= 0};{production= ["E";"T";"E'"];dot= 1};{production= ["E";"T";"E'"];dot= 2};{production= ["E'"];dot= 0};{production= ["E'";"+";"T";"E'"];dot= 0};{production= ["E'";"+";"T";"E'"];dot= 1};{production= ["E'";"+";"T";"E'"];dot= 2};{production= ["E'";"+";"T";"E'"];dot= 3};{production= ["F";"(";"E";")"];dot= 0};{production= ["F";"(";"E";")"];dot= 1};{production= ["F";"(";"E";")"];dot= 2};{production= ["F";"(";"E";")"];dot= 3};{production= ["F";"id"];dot= 0};{production= ["F";"id"];dot= 1};{production= ["T";"F";"T'"];dot= 0};{production= ["T";"F";"T'"];dot= 1};{production= ["T";"F";"T'"];dot= 2};{production= ["T'"];dot= 0};{production= ["T'";"*";"F";"T'"];dot= 0};{production= ["T'";"*";"F";"T'"];dot= 1};{production= ["T'";"*";"F";"T'"];dot= 2};{production= ["T'";"*";"F";"T'"];dot= 3}]
    let itemCoreAttributes = Map [{production= ["";"E"];dot= 0},(true,Set.empty);{production= ["E";"T";"E'"];dot= 0},(true,set ["+"]);{production= ["E";"T";"E'"];dot= 1},(true,Set.empty);{production= ["E'";"+";"T";"E'"];dot= 1},(true,set ["+"]);{production= ["E'";"+";"T";"E'"];dot= 2},(true,Set.empty);{production= ["F";"(";"E";")"];dot= 1},(false,set [")"]);{production= ["T";"F";"T'"];dot= 0},(true,set ["*"]);{production= ["T";"F";"T'"];dot= 1},(true,Set.empty);{production= ["T'";"*";"F";"T'"];dot= 1},(true,set ["*"]);{production= ["T'";"*";"F";"T'"];dot= 2},(true,Set.empty)]
    let kernels = set [set [{production= ["";"E"];dot= 0}];set [{production= ["";"E"];dot= 1}];set [{production= ["E";"T";"E'"];dot= 1}];set [{production= ["E";"T";"E'"];dot= 2}];set [{production= ["E'";"+";"T";"E'"];dot= 1}];set [{production= ["E'";"+";"T";"E'"];dot= 2}];set [{production= ["E'";"+";"T";"E'"];dot= 3}];set [{production= ["F";"(";"E";")"];dot= 1}];set [{production= ["F";"(";"E";")"];dot= 2}];set [{production= ["F";"(";"E";")"];dot= 3}];set [{production= ["F";"id"];dot= 1}];set [{production= ["T";"F";"T'"];dot= 1}];set [{production= ["T";"F";"T'"];dot= 2}];set [{production= ["T'";"*";"F";"T'"];dot= 1}];set [{production= ["T'";"*";"F";"T'"];dot= 2}];set [{production= ["T'";"*";"F";"T'"];dot= 3}]]
    let closures = [set ["(",{production= ["F";"(";"E";")"];dot= 0};"E",{production= ["";"E"];dot= 0};"F",{production= ["T";"F";"T'"];dot= 0};"T",{production= ["E";"T";"E'"];dot= 0};"id",{production= ["F";"id"];dot= 0}];set ["",{production= ["";"E"];dot= 1}];set ["",{production= ["E'"];dot= 0};")",{production= ["E'"];dot= 0};"+",{production= ["E'";"+";"T";"E'"];dot= 0};"E'",{production= ["E";"T";"E'"];dot= 1}];set ["",{production= ["E";"T";"E'"];dot= 2};")",{production= ["E";"T";"E'"];dot= 2}];set ["(",{production= ["F";"(";"E";")"];dot= 0};"F",{production= ["T";"F";"T'"];dot= 0};"T",{production= ["E'";"+";"T";"E'"];dot= 1};"id",{production= ["F";"id"];dot= 0}];set ["",{production= ["E'"];dot= 0};")",{production= ["E'"];dot= 0};"+",{production= ["E'";"+";"T";"E'"];dot= 0};"E'",{production= ["E'";"+";"T";"E'"];dot= 2}];set ["",{production= ["E'";"+";"T";"E'"];dot= 3};")",{production= ["E'";"+";"T";"E'"];dot= 3}];set ["(",{production= ["F";"(";"E";")"];dot= 0};"E",{production= ["F";"(";"E";")"];dot= 1};"F",{production= ["T";"F";"T'"];dot= 0};"T",{production= ["E";"T";"E'"];dot= 0};"id",{production= ["F";"id"];dot= 0}];set [")",{production= ["F";"(";"E";")"];dot= 2}];set ["",{production= ["F";"(";"E";")"];dot= 3};")",{production= ["F";"(";"E";")"];dot= 3};"*",{production= ["F";"(";"E";")"];dot= 3};"+",{production= ["F";"(";"E";")"];dot= 3}];set ["",{production= ["F";"id"];dot= 1};")",{production= ["F";"id"];dot= 1};"*",{production= ["F";"id"];dot= 1};"+",{production= ["F";"id"];dot= 1}];set ["",{production= ["T'"];dot= 0};")",{production= ["T'"];dot= 0};"*",{production= ["T'";"*";"F";"T'"];dot= 0};"+",{production= ["T'"];dot= 0};"T'",{production= ["T";"F";"T'"];dot= 1}];set ["",{production= ["T";"F";"T'"];dot= 2};")",{production= ["T";"F";"T'"];dot= 2};"+",{production= ["T";"F";"T'"];dot= 2}];set ["(",{production= ["F";"(";"E";")"];dot= 0};"F",{production= ["T'";"*";"F";"T'"];dot= 1};"id",{production= ["F";"id"];dot= 0}];set ["",{production= ["T'"];dot= 0};")",{production= ["T'"];dot= 0};"*",{production= ["T'";"*";"F";"T'"];dot= 0};"+",{production= ["T'"];dot= 0};"T'",{production= ["T'";"*";"F";"T'"];dot= 2}];set ["",{production= ["T'";"*";"F";"T'"];dot= 3};")",{production= ["T'";"*";"F";"T'"];dot= 3};"+",{production= ["T'";"*";"F";"T'"];dot= 3}]]
    let gotos = [Map ["(",set [{production= ["F";"(";"E";")"];dot= 1}];"E",set [{production= ["";"E"];dot= 1}];"F",set [{production= ["T";"F";"T'"];dot= 1}];"T",set [{production= ["E";"T";"E'"];dot= 1}];"id",set [{production= ["F";"id"];dot= 1}]];Map.empty;Map ["+",set [{production= ["E'";"+";"T";"E'"];dot= 1}];"E'",set [{production= ["E";"T";"E'"];dot= 2}]];Map.empty;Map ["(",set [{production= ["F";"(";"E";")"];dot= 1}];"F",set [{production= ["T";"F";"T'"];dot= 1}];"T",set [{production= ["E'";"+";"T";"E'"];dot= 2}];"id",set [{production= ["F";"id"];dot= 1}]];Map ["+",set [{production= ["E'";"+";"T";"E'"];dot= 1}];"E'",set [{production= ["E'";"+";"T";"E'"];dot= 3}]];Map.empty;Map ["(",set [{production= ["F";"(";"E";")"];dot= 1}];"E",set [{production= ["F";"(";"E";")"];dot= 2}];"F",set [{production= ["T";"F";"T'"];dot= 1}];"T",set [{production= ["E";"T";"E'"];dot= 1}];"id",set [{production= ["F";"id"];dot= 1}]];Map [")",set [{production= ["F";"(";"E";")"];dot= 3}]];Map.empty;Map.empty;Map ["*",set [{production= ["T'";"*";"F";"T'"];dot= 1}];"T'",set [{production= ["T";"F";"T'"];dot= 2}]];Map.empty;Map ["(",set [{production= ["F";"(";"E";")"];dot= 1}];"F",set [{production= ["T'";"*";"F";"T'"];dot= 2}];"id",set [{production= ["F";"id"];dot= 1}]];Map ["*",set [{production= ["T'";"*";"F";"T'"];dot= 1}];"T'",set [{production= ["T'";"*";"F";"T'"];dot= 3}]];Map.empty]
    let conflicts = [Map ["(",set [{production= ["F";"(";"E";")"];dot= 0}];"E",set [{production= ["";"E"];dot= 0}];"F",set [{production= ["T";"F";"T'"];dot= 0}];"T",set [{production= ["E";"T";"E'"];dot= 0}];"id",set [{production= ["F";"id"];dot= 0}]];Map ["",set [{production= ["";"E"];dot= 1}]];Map ["",set [{production= ["E'"];dot= 0}];")",set [{production= ["E'"];dot= 0}];"+",set [{production= ["E'";"+";"T";"E'"];dot= 0}];"E'",set [{production= ["E";"T";"E'"];dot= 1}]];Map ["",set [{production= ["E";"T";"E'"];dot= 2}];")",set [{production= ["E";"T";"E'"];dot= 2}]];Map ["(",set [{production= ["F";"(";"E";")"];dot= 0}];"F",set [{production= ["T";"F";"T'"];dot= 0}];"T",set [{production= ["E'";"+";"T";"E'"];dot= 1}];"id",set [{production= ["F";"id"];dot= 0}]];Map ["",set [{production= ["E'"];dot= 0}];")",set [{production= ["E'"];dot= 0}];"+",set [{production= ["E'";"+";"T";"E'"];dot= 0}];"E'",set [{production= ["E'";"+";"T";"E'"];dot= 2}]];Map ["",set [{production= ["E'";"+";"T";"E'"];dot= 3}];")",set [{production= ["E'";"+";"T";"E'"];dot= 3}]];Map ["(",set [{production= ["F";"(";"E";")"];dot= 0}];"E",set [{production= ["F";"(";"E";")"];dot= 1}];"F",set [{production= ["T";"F";"T'"];dot= 0}];"T",set [{production= ["E";"T";"E'"];dot= 0}];"id",set [{production= ["F";"id"];dot= 0}]];Map [")",set [{production= ["F";"(";"E";")"];dot= 2}]];Map ["",set [{production= ["F";"(";"E";")"];dot= 3}];")",set [{production= ["F";"(";"E";")"];dot= 3}];"*",set [{production= ["F";"(";"E";")"];dot= 3}];"+",set [{production= ["F";"(";"E";")"];dot= 3}]];Map ["",set [{production= ["F";"id"];dot= 1}];")",set [{production= ["F";"id"];dot= 1}];"*",set [{production= ["F";"id"];dot= 1}];"+",set [{production= ["F";"id"];dot= 1}]];Map ["",set [{production= ["T'"];dot= 0}];")",set [{production= ["T'"];dot= 0}];"*",set [{production= ["T'";"*";"F";"T'"];dot= 0}];"+",set [{production= ["T'"];dot= 0}];"T'",set [{production= ["T";"F";"T'"];dot= 1}]];Map ["",set [{production= ["T";"F";"T'"];dot= 2}];")",set [{production= ["T";"F";"T'"];dot= 2}];"+",set [{production= ["T";"F";"T'"];dot= 2}]];Map ["(",set [{production= ["F";"(";"E";")"];dot= 0}];"F",set [{production= ["T'";"*";"F";"T'"];dot= 1}];"id",set [{production= ["F";"id"];dot= 0}]];Map ["",set [{production= ["T'"];dot= 0}];")",set [{production= ["T'"];dot= 0}];"*",set [{production= ["T'";"*";"F";"T'"];dot= 0}];"+",set [{production= ["T'"];dot= 0}];"T'",set [{production= ["T'";"*";"F";"T'"];dot= 2}]];Map ["",set [{production= ["T'";"*";"F";"T'"];dot= 3}];")",set [{production= ["T'";"*";"F";"T'"];dot= 3}];"+",set [{production= ["T'";"*";"F";"T'"];dot= 3}]]]
    let actions = [Map ["(",Shift(set [{production= ["F";"(";"E";")"];dot= 1}]);"E",Shift(set [{production= ["";"E"];dot= 1}]);"F",Shift(set [{production= ["T";"F";"T'"];dot= 1}]);"T",Shift(set [{production= ["E";"T";"E'"];dot= 1}]);"id",Shift(set [{production= ["F";"id"];dot= 1}])];Map ["",Reduce ["";"E"]];Map ["",Reduce ["E'"];")",Reduce ["E'"];"+",Shift(set [{production= ["E'";"+";"T";"E'"];dot= 1}]);"E'",Shift(set [{production= ["E";"T";"E'"];dot= 2}])];Map ["",Reduce ["E";"T";"E'"];")",Reduce ["E";"T";"E'"]];Map ["(",Shift(set [{production= ["F";"(";"E";")"];dot= 1}]);"F",Shift(set [{production= ["T";"F";"T'"];dot= 1}]);"T",Shift(set [{production= ["E'";"+";"T";"E'"];dot= 2}]);"id",Shift(set [{production= ["F";"id"];dot= 1}])];Map ["",Reduce ["E'"];")",Reduce ["E'"];"+",Shift(set [{production= ["E'";"+";"T";"E'"];dot= 1}]);"E'",Shift(set [{production= ["E'";"+";"T";"E'"];dot= 3}])];Map ["",Reduce ["E'";"+";"T";"E'"];")",Reduce ["E'";"+";"T";"E'"]];Map ["(",Shift(set [{production= ["F";"(";"E";")"];dot= 1}]);"E",Shift(set [{production= ["F";"(";"E";")"];dot= 2}]);"F",Shift(set [{production= ["T";"F";"T'"];dot= 1}]);"T",Shift(set [{production= ["E";"T";"E'"];dot= 1}]);"id",Shift(set [{production= ["F";"id"];dot= 1}])];Map [")",Shift(set [{production= ["F";"(";"E";")"];dot= 3}])];Map ["",Reduce ["F";"(";"E";")"];")",Reduce ["F";"(";"E";")"];"*",Reduce ["F";"(";"E";")"];"+",Reduce ["F";"(";"E";")"]];Map ["",Reduce ["F";"id"];")",Reduce ["F";"id"];"*",Reduce ["F";"id"];"+",Reduce ["F";"id"]];Map ["",Reduce ["T'"];")",Reduce ["T'"];"*",Shift(set [{production= ["T'";"*";"F";"T'"];dot= 1}]);"+",Reduce ["T'"];"T'",Shift(set [{production= ["T";"F";"T'"];dot= 2}])];Map ["",Reduce ["T";"F";"T'"];")",Reduce ["T";"F";"T'"];"+",Reduce ["T";"F";"T'"]];Map ["(",Shift(set [{production= ["F";"(";"E";")"];dot= 1}]);"F",Shift(set [{production= ["T'";"*";"F";"T'"];dot= 2}]);"id",Shift(set [{production= ["F";"id"];dot= 1}])];Map ["",Reduce ["T'"];")",Reduce ["T'"];"*",Shift(set [{production= ["T'";"*";"F";"T'"];dot= 1}]);"+",Reduce ["T'"];"T'",Shift(set [{production= ["T'";"*";"F";"T'"];dot= 3}])];Map ["",Reduce ["T'";"*";"F";"T'"];")",Reduce ["T'";"*";"F";"T'"];"+",Reduce ["T'";"*";"F";"T'"]]]
    let resolvedClosures = [Map [{production= ["";"E"];dot= 0},Set.empty;{production= ["E";"T";"E'"];dot= 0},Set.empty;{production= ["F";"(";"E";")"];dot= 0},Set.empty;{production= ["F";"id"];dot= 0},Set.empty;{production= ["T";"F";"T'"];dot= 0},Set.empty];Map [{production= ["";"E"];dot= 1},set [""]];Map [{production= ["E";"T";"E'"];dot= 1},Set.empty;{production= ["E'"];dot= 0},set ["";")"];{production= ["E'";"+";"T";"E'"];dot= 0},Set.empty];Map [{production= ["E";"T";"E'"];dot= 2},set ["";")"]];Map [{production= ["E'";"+";"T";"E'"];dot= 1},Set.empty;{production= ["F";"(";"E";")"];dot= 0},Set.empty;{production= ["F";"id"];dot= 0},Set.empty;{production= ["T";"F";"T'"];dot= 0},Set.empty];Map [{production= ["E'"];dot= 0},set ["";")"];{production= ["E'";"+";"T";"E'"];dot= 0},Set.empty;{production= ["E'";"+";"T";"E'"];dot= 2},Set.empty];Map [{production= ["E'";"+";"T";"E'"];dot= 3},set ["";")"]];Map [{production= ["E";"T";"E'"];dot= 0},Set.empty;{production= ["F";"(";"E";")"];dot= 0},Set.empty;{production= ["F";"(";"E";")"];dot= 1},Set.empty;{production= ["F";"id"];dot= 0},Set.empty;{production= ["T";"F";"T'"];dot= 0},Set.empty];Map [{production= ["F";"(";"E";")"];dot= 2},Set.empty];Map [{production= ["F";"(";"E";")"];dot= 3},set ["";")";"*";"+"]];Map [{production= ["F";"id"];dot= 1},set ["";")";"*";"+"]];Map [{production= ["T";"F";"T'"];dot= 1},Set.empty;{production= ["T'"];dot= 0},set ["";")";"+"];{production= ["T'";"*";"F";"T'"];dot= 0},Set.empty];Map [{production= ["T";"F";"T'"];dot= 2},set ["";")";"+"]];Map [{production= ["F";"(";"E";")"];dot= 0},Set.empty;{production= ["F";"id"];dot= 0},Set.empty;{production= ["T'";"*";"F";"T'"];dot= 1},Set.empty];Map [{production= ["T'"];dot= 0},set ["";")";"+"];{production= ["T'";"*";"F";"T'"];dot= 0},Set.empty;{production= ["T'";"*";"F";"T'"];dot= 2},Set.empty];Map [{production= ["T'";"*";"F";"T'"];dot= 3},set ["";")";"+"]]]
    let encodedActions = [["(",7;"E",1;"F",11;"T",2;"id",10];["",0];["",-2;")",-2;"+",4;"E'",3];["",-1;")",-1];["(",7;"F",11;"T",5;"id",10];["",-2;")",-2;"+",4;"E'",6];["",-3;")",-3];["(",7;"E",8;"F",11;"T",2;"id",10];[")",9];["",-4;")",-4;"*",-4;"+",-4];["",-5;")",-5;"*",-5;"+",-5];["",-7;")",-7;"*",13;"+",-7;"T'",12];["",-6;")",-6;"+",-6];["(",7;"F",14;"id",10];["",-7;")",-7;"*",13;"+",-7;"T'",15];["",-8;")",-8;"+",-8]]
    let encodedClosures = [[0,0,[];-1,0,[];-4,0,[];-5,0,[];-6,0,[]];[0,1,[""]];[-1,1,[];-2,0,["";")"];-3,0,[]];[-1,2,["";")"]];[-3,1,[];-4,0,[];-5,0,[];-6,0,[]];[-2,0,["";")"];-3,0,[];-3,2,[]];[-3,3,["";")"]];[-1,0,[];-4,0,[];-4,1,[];-5,0,[];-6,0,[]];[-4,2,[]];[-4,3,["";")";"*";"+"]];[-5,1,["";")";"*";"+"]];[-6,1,[];-7,0,["";")";"+"];-8,0,[]];[-6,2,["";")";"+"]];[-4,0,[];-5,0,[];-8,1,[]];[-7,0,["";")";"+"];-8,0,[];-8,2,[]];[-8,3,["";")";"+"]]]
