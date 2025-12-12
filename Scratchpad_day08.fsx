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

type Vector = { X: int64; Y: int64; Z: int64 }
let parseVector (line: string) =
    let parts = line.Split(',')
    { X = int64 parts[0]; Y = int64 parts[1]; Z = int64 parts[2] }

let vectorPairs (vectors: Vector array) =
    seq {
        for i in 0 .. vectors.Length - 2 do
            for j in i + 1 .. vectors.Length - 1 do
                yield (vectors[i], vectors[j])
    }

let calculateDistances (vectorPairs: (Vector * Vector) seq) =
    vectorPairs 
    |> Seq.map (fun (v1, v2) ->
        let dx = v1.X - v2.X
        let dy = v1.Y - v2.Y
        let dz = v1.Z - v2.Z
        let distance = Math.Sqrt(float(dx * dx + dy * dy + dz * dz))
        (v1, v2, distance)
    ) 
    |> Seq.sortBy (fun (_, _, distance) -> distance)

let getNeighbours (distances: (Vector * Vector * float) seq) =
    let rec findNeighbours (distancesList: (Vector * Vector * float) list) (closeNeighbours: Vector HashSet list) =
        match distancesList with
        | [] -> closeNeighbours
        | (vector1, vector2, dist)::tail ->
            let set1 = closeNeighbours |> List.tryFind (fun hs -> hs.Contains(vector1))
            let set2 = closeNeighbours |> List.tryFind (fun hs -> hs.Contains(vector2))
            
            match set1, set2 with
            | Some s1, Some s2 when obj.ReferenceEquals(s1, s2) ->
                // Already in the same set
                findNeighbours tail closeNeighbours
            | Some s1, Some s2 ->
                // Different sets - merge them
                s2 |> Seq.iter (s1.Add >> ignore)
                let newNeighbours = closeNeighbours |> List.filter (fun hs -> not (obj.ReferenceEquals(hs, s2)))
                findNeighbours tail newNeighbours
            | Some s1, None ->
                // vector2 not in any set, add to s1
                s1.Add(vector2) |> ignore
                findNeighbours tail closeNeighbours
            | None, Some s2 ->
                // vector1 not in any set, add to s2
                s2.Add(vector1) |> ignore
                findNeighbours tail closeNeighbours
            | None, None ->
                // Neither in any set, create new set
                let newSet = HashSet<Vector>()
                newSet.Add(vector1) |> ignore
                newSet.Add(vector2) |> ignore
                findNeighbours tail (newSet :: closeNeighbours)

    findNeighbours (distances |> Seq.toList) []



// part 1
getInput 8
// getTestInput 8
|> Array.map parseVector
|> vectorPairs
|> calculateDistances
|> fun x -> 
    let count = Math.Min(1000, x |> Seq.length)
    Seq.take count x
|> Seq.toList
|> getNeighbours
|> List.sortByDescending (fun hs -> hs.Count)
|> List.take 3
|> List.fold (fun acc hs -> acc * int64 hs.Count) 1L

// part 2
// getInput 8
// getTestInput 8

