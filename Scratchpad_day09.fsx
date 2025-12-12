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

let parseInput (line:string) =
    line.Split(',')
    |> Array.map int64

let findFurthestEdges (inputs:int64[][]) =
    // calculate distance between each point (2d)
    let distances =
        [|
            for i in 0 .. inputs.Length - 1 do
                for j in i + 1 .. inputs.Length - 1 do
                    let p1 = inputs[i]
                    let p2 = inputs[j]
                    let dist =
                        Math.Sqrt(
                            p1
                            |> Array.zip p2
                            |> Array.sumBy (fun (a, b) -> float ((a - b) * (a - b)))
                        )
                    yield (dist, (p1, p2))
        |]
    distances

// part 1
getInput 9
// getTestInput 9
|> Array.map parseInput
|> findFurthestEdges
|> Array.maxBy fst
|> fun (_, (p1, p2)) ->
    printfn "%A %A" (Math.Max(p1[0],p2[0]) - Math.Min(p1[0],p2[0]) + 1L) (Math.Max(p1[1], p2[1]) - Math.Min(p1[1], p2[1]) + 1L)
    (Math.Max(p1[0],p2[0]) - Math.Min(p1[0],p2[0]) + 1L) * (Math.Max(p1[1], p2[1]) - Math.Min(p1[1], p2[1]) + 1L)

// part 2
// getInput 9
// getTestInput 9

