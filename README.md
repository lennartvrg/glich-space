# glich.space

glich.space is the semester project for the winter semester 2019 at CODE. This repository
contains all of the source code and assets for the game.

## Getting Started

After cloning the repository it strictly necessary to initialize all submodules via `git submodule update --init --recursive`.
When first opening the Unity3D project **DO NOT** run Auto Update API as this is will break stuff.

## Technology stack

We are using [Unity3D](https://unity.com/) as our game engine of choice. As we are a developing a VR game,
we choose to use [VRTK](https://vrtoolkit.readme.io/) to provide us with a common interface for a variety of VR headsets.

## Collaboration

We use Git (who would have thought) as our version control system. In addition we use the Git LFS addon to
work with large asset and texture files.

Development is split across three permanent branches and feature branches for implementing functionality.
- **feature/**: Active development itself is done on feature branches. These branches should have descriptive
names and should only implement one feature at a time.
- **master**: Once a features is finished, it will be merge into master via a pull request. PRs require at least one
reviewer and should only be merged when all comments are resolved.
- **staging**: Once development is at a point where the game is ready for play testing, the master branch shall be
merged into this branch via a pull request.
- **production**: Once play testing has been successful and the game/level is enjoyable to play, it may be merged
into the production branch. The state in this branch should be stable enough to present the game to the general public.


Within Unity3D we have three separate scenes for each team member. Whenever editing assets or prefabs each team member
stays within their own scene. This is necessary as it is quite difficult to merge unity scenes.
