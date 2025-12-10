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

let findTachyonSplits (inputLines: string array) =
    let rec traverseLines (lines: string array) (index:int) (splitCount:int) =
        // We can start directly on line 1 (since 0 only has the starting point S)
        // we return once we have reached the end of the lines and return the split count
        // We look for tachyon characters one line above "|" or "S" (starting point)
        // We check if on the current line for the tachyon beams we hit a splitter "^" -> if so we will palce a tachyon beam on x-1 and x+1 on the current line and increment the split count
        if index >= lines.Length then
            splitCount
        else
            let previousLine = lines[index - 1]


            let tachyonPositions =
                previousLine
                |> Seq.mapi (fun i c -> (i, c))
                |> Seq.filter (fun (_, c) -> c = '|' || c = 'S')
                |> Seq.map fst
                |> Seq.toList

            let mutable newSplitCount = splitCount

            for pos in tachyonPositions do
                if pos > 0 && lines[index][pos] = '^' then
                    let chars = lines[index].ToCharArray()
                    chars[pos - 1] <- '|'
                    chars[pos + 1] <- '|'
                    lines[index] <- (chars |> String)
                    newSplitCount <- newSplitCount + 1
                else
                    let chars = lines[index].ToCharArray()
                    chars[pos] <- '|'
                    lines[index] <- (chars |> String)

            traverseLines lines (index + 1) newSplitCount

    traverseLines inputLines 1 0


// part 1
getInput 7
// getTestInput 7
|> findTachyonSplits

let findTachyons (inputLines: string array) =
    let map =
        inputLines
        |> Array.map (fun line ->
            line
            |> Seq.map (fun c -> (string c, 0L))
            |> Array.ofSeq
        )
    let rec traverseLines (map: (string * int64) array array) (index:int)  =
        if index >= map.Length then
            map[map.Length - 1] |> Array.sumBy snd
        else
            let previousLine = map[index - 1]


            let tachyonPositions =
                previousLine
                |> Seq.mapi (fun i c -> (i, c))
                |> Seq.filter (fun (_, (c, _)) -> c = "|" || c = "S")
                |> Seq.map fst
                |> Seq.toList

            for pos in tachyonPositions do
                if pos > 0 && map[index][pos] |> fst = "^" then
                    map[index][pos - 1] <- "|", (map[index][pos - 1] |> snd) + (map[index - 1][pos] |> snd)
                    map[index][pos + 1] <- "|", (map[index][pos + 1] |> snd) + (map[index - 1][pos] |> snd)

                else
                    map[index][pos] <- "|", (map[index][pos] |> snd) + Math.Max(map[index-1][pos] |> snd, 1L)

            traverseLines map (index + 1)

    traverseLines map 1

// part 2
getInput 7
// getTestInput 7
|> findTachyons

