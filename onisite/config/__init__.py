from pathlib import Path

# Python has built-in TOML support starting in 3.11
#  Ref: https://docs.python.org/3/library/tomllib.html
# Until then, fallback to tomli, which is the basis for tomlib
try:
    import tomlib
except ModuleNotFoundError:
    import tomli as tomlib


local_path = Path(__file__).parent / "local.toml"
with local_path.open(mode="rb") as lp:
    local_config = tomlib.load(lp)

