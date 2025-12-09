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

let parseMathEquations (inputLines: string array) =
    let inputLinesAndColumns =
        inputLines
        |> Array.map (fun line -> line.Split(" ") |> Array.filter (fun s -> s <> ""))

    let rows = inputLinesAndColumns.Length
    let cols = inputLinesAndColumns[0].Length

    let equations =
        [| 
            for y in 0 .. cols - 1 do
                yield [| 
                    for x in 0 .. rows - 1 do 
                        yield inputLinesAndColumns[x][y] |] |> Array.rev

        |]
    equations


// part 1
getInput 6
// getTestInput 6
|> parseMathEquations
|>  Array.map (fun equation -> (equation |> Array.head, equation |> Array.tail |> Array.map int64))
|> Array.map (fun (operation, numbers) -> 
    match operation with
    | "+" -> numbers |> Array.sum
    | "*" -> numbers |> Array.fold (fun acc n -> acc * n) 1
    | _ -> failwith "unknown operation")
|> Array.sum

let parseCephalopodMathEquations (inputLines: string array) =
    let blockIndexes = 
        inputLines
        |> Seq.rev
        |> Seq.head
        |> fun s -> s.ToCharArray() |> Array.map string
        |> Seq.mapi (fun i char -> if not (String.IsNullOrWhiteSpace char) then Some i else None)
        |> Seq.choose id
        |> Seq.toArray

    let blockIndexRanges =
        blockIndexes
        |> Array.pairwise
        |> Array.map (fun (startIdx, endIdx) -> (startIdx, endIdx - 2))
        |> fun x -> Array.append x  [| (blockIndexes |> Array.last, ((inputLines |> Seq.head |> fun s -> s.ToCharArray() |> Array.length) - 1)) |]

    let inputLinesAndColumns =
        inputLines
        |> Array.mapi (fun i line -> 
            if i = inputLines.Length - 1 then
                line.Split(" ") |> Array.filter (fun s -> s <> "")
            else
                blockIndexRanges|> Array.map (fun (startIdx, endIdx) -> line.Substring(startIdx, endIdx - startIdx + 1)
            )
        )
        
    inputLinesAndColumns
    |> fun inputLinesAndColumns ->
        let inputNumbers = 
            [| for x in 0 .. (inputLinesAndColumns[0] |> Array.length) - 1 do
                [| for y in 0 .. (inputLinesAndColumns |> Array.length) - 2 do
                    yield ((inputLinesAndColumns[y][x]).ToCharArray() |> Array.map string) |]
                |> (fun inputDigits ->  
                        [| for x in 0 .. inputDigits[0].Length - 1 do
                            [|for y in 0 .. inputDigits.Length - 1 do 
                                yield inputDigits[y][x] 
                            |] |> String.Concat |> int64
                        |]
                    )
                            |]
        inputNumbers
    |> Array.mapi (fun i line -> (inputLinesAndColumns |> Array.last)[i], line)

// part 2
getInput 6
// getTestInput 6
|> parseCephalopodMathEquations
|> Array.map (fun (operation, numbers) -> 
    match operation with
    | "+" -> numbers |> Array.sum
    | "*" -> numbers |> Array.fold (fun acc n -> acc * n) 1
    | _ -> failwith "unknown operation")
|> Array.sum
