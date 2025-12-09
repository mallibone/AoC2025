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

let parseInput (inputLines: string seq) =
    let freshIngredientIdRanges = 
        inputLines 
        |> Seq.takeWhile (fun line -> not (String.IsNullOrWhiteSpace line))
        |> Seq.map (fun line -> 
            let parts = line.Split('-')
            (int64 parts.[0], int64 parts.[1]))
        |> Seq.toArray

    
    let ingredientIds =
        inputLines
        |> Seq.skip ((inputLines |> Seq.findIndex (fun line -> String.IsNullOrWhiteSpace line)) + 1)
        |> Seq.map int64
        |> Seq.toArray

    freshIngredientIdRanges, ingredientIds
    
let findFreshIngredients (freshIngredientIdRanges: (int64 * int64)[], ingredientIds: int64[]) =
    ingredientIds
    |> Array.filter (fun id -> 
        freshIngredientIdRanges 
        |> Array.exists (fun (startId, endId) -> id >= startId && id <= endId))

// part 1
getInput 5
// getTestInput 5
|> parseInput
|> findFreshIngredients
|> Array.length

let mergeRanges (freshIngredientIdRanges: (int64 * int64)[]) =
    freshIngredientIdRanges
    |> Array.sortBy fst
    |> Array.fold (fun acc (startId, endId) ->
        match acc with
        | [] -> [ (startId, endId) ]
        | (lastStart, lastEnd) :: tail when startId <= lastEnd + 1L ->
            (lastStart, max lastEnd endId) :: tail
        | _ ->
            (startId, endId) :: acc
    ) []
    |> Array.ofList

// part 2
getInput 5
// getTestInput 5
|> parseInput
|> fst
|> mergeRanges
|> Array.map (fun (startId, endId) -> endId - startId + 1L)
|> Array.sum


