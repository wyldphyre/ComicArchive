# Examples of comic archive file names encountered in the wild

This file contains examples of the file names of various comic archives I've encountered. The aim is to have a list that represents all the possible file name structures that, ideally, need to be handled when attempting to extract useful in formation about an archive from the file name.

The lists are broken into formats I consider common, and ones I consider uncommon.

## Common file name structures

- Vampeerz v1 ch01.cbz
- Assassination Classroom v01 (2014) (Digital) (Lovag-Empire).cbz
- Assassination Classroom v01.cbz
- Assassination Classroom 01.cbz
- Assassination Classroom ch01.cbz
- Assassination Classroom ch 01.cbz
- Ah My Goddess 1.cbz
- Ah My Goddess 10.cbz
- Claymore 001 - Silver-eyed Slayer[m-s].cbz : includes chapter name
- Claymore 002 - Claws in the Sky[m-s].cbz : includes chapter name
- [Tokuwotsumu] Tea Brown and Milk Tea [TZdY].cbz
- (Isoya Yuki) The Day the Cherryfruit Ripens (Hirari 14) [yuriproject].cbz
- [Garun] I Could Just Tell.cbz
- [Takemiya Jin] Yaezakura Sympathy 1 [TZdY].cbz
- 04.cbz
- 05 - Let's Be Careful With Summer.cbz : assume chapter number and name
- 2000 AD 0001.cbz
- 2000 AD 0345 (Cclay).cbz
- Ascender 001 (2019) (Digital) (Zone-Empire).cbz

## Uncommon file name structures

Some of these can probably be handled programatically, but I think others should be left and require that they be cleaned up first.

- [Tonari no Kobeya] Edible Flowers [Ch01].cbz
- [Tonari no Kobeya] Edible Flowers {Ch03}.cbz
- [Tonari no Kobeya] Edible Flowers ch05 v2.cbz
- [Tonari no Kobeya] Edible Flowers Ch06.cbz
- Mononoke Sharing Ch.01.cbz
- Bitter_Trap_[Aerandria].cbz
- Blue Dragon (complete).cbz
- all.you.need.is.kill.MangaReader.all.you.need.is.kill.001..kiriya.keij.cbz
- mysterious.girlfriend.x.MangaHere.v001.c000.cbz
- mysterious.girlfriend.x.MangaHere.v005.c031.cbz
- (Hamano Ringo) Cotton Candy part 1 (Gazette Vol. 1) [yuriproject].cbz
- [Ponpon-O (Mountain Pukuichi)] Futari Nara [English]-1280x.cbz
- [Tokuwotsumu] Tea Brown and Milk Tea [TZdY].cbz
- dengeki.daisy.MangaReader.dengeki.daisy.001..suspicious.guy.cbz : includes chapter name
- dengeki.daisy.MangaReader.dengeki.daisy.002..even.for.an.instant.he.is.a.hero.cbz : includes chapter name
- 01 Prologue_-_Hidden_Name_[lililicious].cbz : think this can be assumed to be in a folder named for the archive


## Special Cases

These are examples that would probably be difficult to impossible to work out automatically.

- Change 123.cbz : This is the full name of the manga, not a series name with an issue/chapter number