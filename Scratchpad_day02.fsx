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

let parseRanges (input:string) = input.Split(",")

let createRanges (range:string) =
    let rangeMinMax = range.Split("-")
    let min = rangeMinMax[0] |> int64
    let max = rangeMinMax[1] |> int64
    let count = int(max - min) + 1

    Array.init (count) (fun i -> min + int64 i |> string)

let findDummyValues (ranges:string[]) =
    ranges 
    |> Array.filter (fun s -> 
        s.Length % 2 = 0 && 
        let half = s.Length / 2
        s.Substring(0, half) = s.Substring(half)
    )

let findDummyValuesII (ranges:string[]) =
    // additionally find ggTs for length, check if ggT windows are recuring i.e. 123123123 one ggT is 3x3 123 repeats so is a dummy

    ranges 
    |> Array.filter (fun s -> 
        s.Length % 2 = 0 && 
        let half = s.Length / 2
        s.Substring(0, half) = s.Substring(half)
    )

// part 1
getInput 2
// getTestInput 2
|> Array.collect parseRanges
|> Array.map createRanges
|> Array.collect findDummyValues
|> Array.map int64
|> Array.sum

// part 2
// getInput 2
// getTestInput 2

