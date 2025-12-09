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

let parseToMap (input: string) =
    input.ToCharArray()
    |> Array.map string

let neighborCount (map: string array array) (x:int) (y:int) =
    let rows = map.Length
    let cols = map.[0].Length

    let directions = [| (-1, 0); (-1, -1); (-1, 1); (1, 0); (1, -1); (1, 1); (0, -1); (0, 1) |]

    let validNeighcoords = 
        directions 
        |> Array.map (fun (dx, dy) -> (x + dx, y + dy))
        |> Array.filter (fun (newX, newY) -> newX >= 0 && newX < rows && newY >= 0 && newY < cols)
    
    let foundNeighbours = 
        validNeighcoords 
        |> Array.filter (fun (newX, newY) -> map.[newX].[newY] = "@")
        |> Array.length

    // printfn "%A %A %A %A" foundNeighbours x y validNeighcoords

    foundNeighbours

let findPaperRollsThatCanBeMoved (map: string array array) =
    let rows = map.Length
    let cols = map.[0].Length

    let mutable count = 0

    for x in 0 .. rows - 1 do
        for y in 0 .. cols - 1 do
            if map[x][y] = "@" && neighborCount map x y < 4 then
                count <- count + 1

    count

// part 1
// getInput 4
getTestInput 4
|> Array.map parseToMap
|> findPaperRollsThatCanBeMoved

// part 2
// getInput 4
// getTestInput 4

