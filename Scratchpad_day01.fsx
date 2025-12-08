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
            // {Id = int blueprintId; OreRobotOreCost = int oreRobotCost; ClayRobotOreCost = int clayRobotCost; ObsidianRobotOreCost = int obsidianRobotOreCost; ObsidianRobotClayCost = int obsidianRobotClayCost; GeodeRobotObsidianCost = int geodeRobotObsidianCost; GeodeRobotOreCost = int geodeRobotOreCost}

let parseRotation (line:string) =
    let dir = line[0]
    let valueStr = if line.Length > 1 then line.Substring(1) else "0"
    let value = try int valueStr with _ -> 0
    match dir with
    | 'L' -> -value
    | 'R' -> value
    | _ -> 0


let calcRotationPos (pos:int, zeroPosCounter:int) (rotation:int) =
    let modRotation = rotation % 100
    let newPos = pos + modRotation
    let finalNewPos = (if newPos < 0 then newPos + 100 else newPos) % 100
    // printfn "Rotation: %A, New pos: %A" rotation finalNewPos
    if finalNewPos = 0 then
        (finalNewPos, zeroPosCounter + 1)
    else
        (finalNewPos, zeroPosCounter)


type Direction = L | R
type Rotation = { Direction: Direction; Value: int }

let parseRotationII (line:string) =
    let dir = line[0]
    let valueStr = if line.Length > 1 then line.Substring(1) else "0"
    let value = try int valueStr with _ -> 0
    match dir with
    | 'L' -> {Direction = L; Value = value}
    | 'R' -> {Direction = R; Value = value}
    | _ -> {Direction = R; Value = 0}

let calcRotationPosII (pos:int, zeroPosCounter:int) (rotation:Rotation) =

    let rec rotate currentPos direction clickCount zeroClicks =
        if clickCount = 0 then
            printfn "Current pos: %A, Direction: %A, Clicks: %A, Zero clicks: %A" pos direction rotation.Value zeroClicks
            currentPos, zeroClicks
        else
            let step = if direction = L then -1 else 1
            let nextPos = (currentPos + step + 100) % 100
            if nextPos = 0 then
                rotate nextPos direction (clickCount - 1) (zeroClicks+1)
            else
                rotate nextPos direction (clickCount - 1) zeroClicks

    let (currentPos, zeroClicks) = rotate pos rotation.Direction rotation.Value 0

    (currentPos, zeroPosCounter + zeroClicks)

// 
// part 1
getInput 1
// getTestInput 1
|> Array.map parseRotation
|> Seq.fold calcRotationPos (50, 0)
|> fun (_, zeroCounter) -> printfn "Zero counter: %A" zeroCounter


// part 2
getInput 1
// getTestInput 1
|> Array.map parseRotationII
|> Seq.fold calcRotationPosII (50, 0)
|> fun (_, zeroCounter) -> printfn "Zero counter: %A" zeroCounter

