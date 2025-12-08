#time
#r "nuget: FSharp.Collections.ParallelSeq, 1.2.0"
open System.IO
open System.Text.RegularExpressions
open System
open System.Linq
open System.Collections.Generic
open FSharp.Collections.ParallelSeq
open System.Collections.Concurrent

let getTestInput (day:int) =
    let filename = Path.Combine(__SOURCE_DIRECTORY__, $"Input/TestDay{day:D2}.txt")
    File.ReadAllLines(filename)
    // File.ReadAllText(filename)

let getInput (day:int) =
    let filename = Path.Combine(__SOURCE_DIRECTORY__, $"Input/Day{day:D2}.txt")
    File.ReadAllLines(filename)
    // File.ReadAllText(filename)


// let (|Match|_|) (pat: string) (inp: string) =
//     let m = Regex.Match(inp, pat) in

//     if m.Success then
//         Some(List.tail [ for g in m.Groups -> g.Value ])
//     else
//         None

// let parseInput inputLine =
//     inputLine |> function
//         | Match "Blueprint (.*): Each ore robot costs (.*) ore. Each clay robot costs (.*) ore. Each obsidian robot costs (.*) ore and (.*) clay. Each geode robot costs (.*) ore and (.*) obsidian." [ blueprintId; oreRobotCost; clayRobotCost; obsidianRobotOreCost; obsidianRobotClayCost; geodeRobotOreCost; geodeRobotObsidianCost; ] ->
//             {Id = int blueprintId; OreRobotOreCost = int oreRobotCost; ClayRobotOreCost = int clayRobotCost; ObsidianRobotOreCost = int obsidianRobotOreCost; ObsidianRobotClayCost = int obsidianRobotClayCost; GeodeRobotObsidianCost = int geodeRobotObsidianCost; GeodeRobotOreCost = int geodeRobotOreCost}

let findHighestJolt (input: string) =
    let inputNumbers = 
        input.ToCharArray()
        |> Array.map string
        |> Array.map int

    let highestJolt = inputNumbers.[.. inputNumbers.Length - 2] |> Array.max
    let hightestJoltIndex = inputNumbers |> Array.findIndex (fun x -> x = highestJolt)
    let nextHighestJolt = inputNumbers.[hightestJoltIndex + 1..] |> Array.max
    // printfn "%A" (highestJolt * 10 + nextHighestJolt)

    highestJolt * 10 + nextHighestJolt

let findHighestJoltII (input: string) =
    let inputNumbers = 
        input.ToCharArray()
        |> Array.map string
        |> Array.map int64

    let rec findHighestJoltDigits (input: int64 array) (digitsLeft:int) (highestJolt:int64) =
        if digitsLeft = 0 then
            highestJolt
        else
            printfn "%A %A %A" digitsLeft highestJolt input.Length
            let nextHighestJolt = input.[.. input.Length - (digitsLeft)] |> Array.max
            let hightestJoltIndex = input |> Array.findIndex (fun x -> x = nextHighestJolt)
            // let nextHighestJolt = inputNumbers.[hightestJoltIndex + 1..] |> Array.max
            // printfn "%A" (highestJolt * 10 + nextHighestJolt)

            findHighestJoltDigits (input.[hightestJoltIndex + 1..]) (digitsLeft-1) (highestJolt * 10L + nextHighestJolt)
    findHighestJoltDigits inputNumbers 12 0L

// find the highest number value
// find the index
// index + 1 find the next highest number value
// return the double digit number

// part 1
getInput 3
// getTestInput 3
|> Array.map findHighestJolt
|> Array.sum

// part 2
// getInput 3
getTestInput 3
|> Array.map findHighestJoltII


let inputNumbers = [|9; 8; 7; 6; 5; 4; 3; 2; 1; 1; 1; 1; 1; 1; 1|]
let highestJolt =  inputNumbers.[.. inputNumbers.Length - 12] |> Array.max
// let hightestJoltIndex = inputNumbers |> Array.findIndex (fun x -> x = highestJolt)
// let nextHighestJolt = inputNumbers.[hightestJoltIndex + 1..] |> Array.max
// highestJolt * 10 + nextHighestJolt