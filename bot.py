import os
import discord
from discord.ext import commands
from dotenv import load_dotenv

load_dotenv()
TOKEN = os.getenv('DISCORD_TOKEN')

bot = commands.Bot(command_prefix='!')

@bot.event
async def on_ready():
    print(f'{bot.user.name} has connected to Discord!')

@bot.command(name='luffy')
async def luffy(ctx):
    response = "Monkey D. Luffy is the captain of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='zoro')
async def zoro(ctx):
    response = "Roronoa Zoro is the swordsman of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='nami')
async def nami(ctx):
    response = "Nami is the navigator of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='sanji')
async def sanji(ctx):
    response = "Sanji is the cook of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='chopper')
async def chopper(ctx):
    response = "Tony Tony Chopper is the doctor of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='robin')
async def robin(ctx):
    response = "Nico Robin is the archaeologist of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='franky')
async def franky(ctx):
    response = "Franky is the shipwright of the Thousand Sunny."
    await ctx.send(response)

@bot.command(name='brook')
async def brook(ctx):
    response = "Brook is the musician of the Straw Hat Pirates."
    await ctx.send(response)

@bot.command(name='jinbe')
async def jinbe(ctx):
    response = "Jinbe is the helmsman of the Straw Hat Pirates."
    await ctx.send(response)

bot.run(TOKEN)
