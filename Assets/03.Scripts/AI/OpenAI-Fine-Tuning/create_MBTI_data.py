import openai
import random
import json

openai.api_key = 'YOUR-API-KEY'
model = "gpt-3.5-turbo"

# Broadening the range of topics and tones
topics = ["personal growth", "culture", "everyday life", "relationships", "school life", "hobbies", "career", "work life", "entertainment", "technology"]
tones = ["extroverted", "analytical", "critical", "informative", "Spontaneous"]
# tones = ["analytical", "humorous", "inspirational", "critical", "informative", "introverted", "extroverted", "Planned", "Spontaneous"]

json_data = []

for i in range(1000):  # Assuming you want to generate a set for each topic
    topic = random.choice(topics)
    tone = random.choice(tones)  # Cycle through tones
    
    messages = [
        {"role": "system", "content": f"You are a person whose MBTI is ESTP. Generate a Q&A pair on the topic of {topic}, with a {tone} tone. The questions should start with 'Q:' and the answers should start with 'A:'. Both questions and answers should be in Korean, reflecting deep insight and thoughtful analysis inherent to INTJ personalities. Avoid mentioning MBTI types directly in the questions and answers. Both questions and answers should not exceed 400 characters, respectively."},
        {"role": "user", "content": f"Please provide an example of a Q&A pair discussing {topic} in a {tone} manner. Do not translate or use languages other than Korean."}
    ]
    
    response = openai.chat.completions.create(
        model = model,
        messages = messages
    )
    answer = response.choices[0].message.content

    q_and_a = answer.split('\n')  # This assumes the question and answer are separated by a newline
    if len(q_and_a) >= 2:
        question = q_and_a[0][3:]  # Remove 'Q: '
        answer_text = q_and_a[1][3:]  # Remove 'A: '
        # Append to json_data
        json_data.append({
            "instruction": question,
            "input": "",
            "output": answer_text
        })

# Once the loop is done, you can write json_data to a file
with open('ESTP.json', 'w', encoding='utf-8') as f:
    json.dump(json_data, f, ensure_ascii=False, indent=2)