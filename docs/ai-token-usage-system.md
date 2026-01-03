# AI Token Usage System - Backend Overview

**Last Updated:** January 3, 2026

## 📁 Folder Structure

```
src/Backend.Core/
└── Domain/
    ├── AI/
    │   ├── Usage/                          [Token Management & Logging]
    │   │   ├── TokenDeductionService.cs   [Core Service for Token Management]
    │   │   ├── AIUsageLogRepo.cs           [Database Access & Analytics]
    │   │   ├── AIUsageLog.cs               [Data Model]
    │   │   └── AIUsageMap.cs               [NHibernate Mapping]
    │   │
    │   ├── Models/                         [AI Model Management]
    │   │   ├── AiModelRegistry.cs          [Model Registry & Caching]
    │   │   ├── AiModelWhitelist.cs         [Whitelist Model]
    │   │   ├── AiModelWhitelistRepo.cs     [Database Access]
    │   │   ├── AiModelCache.cs             [In-Memory Cache]
    │   │   ├── AiModelInfo.cs              [Model Info]
    │   │   ├── AiModelProvider.cs          [Provider Enum]
    │   │   └── Persistence/                [EF Core Mappings]
    │   │
    │   ├── AiPageGenerator.cs              [Page Generation with Token Logging]
    │   ├── GenerateFlashCard.cs            [Flashcard Generation with Token Logging]
    │   ├── ClaudeService.cs                [Claude API Integration]
    │   ├── ChatGPTService.cs               [ChatGPT API Integration]
    │   ├── AnthropicApi.cs                 [Anthropic API Response Models]
    │   └── WebContentFetcher.cs            [Web Content Fetcher]
    │
    └── Stripe/
        └── WebhookEventHandler.cs          [Subscription Token Grant/Refresh]

src/Backend.Core/Infrastructure/
└── Cache/
    └── SlidingCache/
        ├── MonthlyTokenUsage.cs            [Monthly Token Tracking Cache]
        └── ExtendedUserCache.cs            [User Cache with Token Usage]

src/Backend.Api/
└── Api/NuxtUI/
    ├── Stores/
    │   ├── PageStoreController.cs          [GenerateFlashcard Endpoint]
    │   └── UserStoreController.cs          [Token Balance Query]
    └── Components/Page/AI/
        └── AiCreatePageController.cs       [Page Generation Endpoints]
```

---

## 🎯 Core Services & Their Functions

### 1. **TokenDeductionService** 
📍 `src/Backend.Core/Domain/AI/Usage/TokenDeductionService.cs`

**Purpose:** Central service for token management (deductions, balance, affordability checks)

**Main Methods:**
| Method | Purpose |
|---------|---------|
| `CanAffordTokens()` | 3 overloads for different scenarios - checks if user has enough tokens |
| `DeductTokens()` | Deducts tokens from user (subscription first, then paid) |
| `HasEnoughTokens()` | Simple check if enough tokens available |
| `GetTotalTokenBalance()` | Returns current balance |
| `GrantInitialSubscriptionTokens()` | Grants initial tokens for new subscriber |
| `RefreshMonthlySubscriptionTokens()` | Resets monthly balance (not accumulated) |

**Key Features:**
- ✅ Token cost calculation with model-specific multipliers (e.g., Claude Opus costs more)
- ✅ Two token types: `SubscriptionTokensBalance` & `PaidTokensBalance`
- ✅ Token estimation from text (approx. 3.5 characters = 1 token)
- ✅ Optimistic locking against race conditions
- ✅ Detailed logging for audit trail

**GenerationType Enum:**
```csharp
PageShort → 500 Output Tokens
PageMedium → 1000 Output Tokens
PageLong → 2000 Output Tokens
WikiWithSubpages → 4000 Output Tokens
Flashcards → 800 Output Tokens
```

---

### 2. **AiUsageLogRepo**
📍 `src/Backend.Core/Domain/AI/Usage/AIUsageLogRepo.cs`

**Purpose:** Database access for token usage logs + analytics & cost tracking

**Main Methods:**
| Method | Purpose |
|---------|---------|
| `AddUsage()` | Logs token usage & deducts automatically |
| `GetTokenUsageForUserFromPastNDays()` | 30-day token history for user |
| `GetUsageWithCosts()` | Detailed logs with USD costs per request |
| `GetCostSummaryByModel()` | Aggregated costs per AI model |
| `GetTotalCosts()` | Aggregated total costs |

**Key Features:**
- 💰 Automatic USD cost calculation (based on model pricing)
- 📊 Token cost multiplier considered
- 🔗 Joins with `aimodelwhitelist` for dynamic price calculation
- 📈 Aggregate query support for analytics

**Return Types:**
- `AiUsageWithCost` - Single log entry with costs
- `AiModelCostSummary` - Costs per model
- `AiTotalCostSummary` - Total costs

---

### 3. **AiModelRegistry**
📍 `src/Backend.Core/Domain/AI/Models/AiModelRegistry.cs`

**Purpose:** Management & caching of whitelisted AI models + admin import

**Main Methods:**
| Method | Purpose |
|---------|---------|
| `GetEnabledModels()` | All enabled models (from cache) |
| `GetAllModels()` | All models for admin |
| `GetModel(modelId)` | Single model by ID |
| `GetTokenCostMultiplier()` | Token cost multiplier for model |
| `IsModelAllowed()` | Checks if model is whitelisted & enabled |
| `FetchAnthropicModelsAsync()` | Imports models from Anthropic API |
| `FetchOpenAiModelsAsync()` | Imports models from OpenAI API |

**Key Features:**
- 🔒 Cache-first approach with DB fallback
- 📡 Automatic API import of model lists
- 🎛️ Token cost multiplier support

---

### 4. **MonthlyTokenUsage**
📍 `src/Backend.Core/Infrastructure/Cache/SlidingCache/MonthlyTokenUsage.cs`

**Purpose:** In-memory caching of monthly token usage per user

**Main Methods:**
| Method | Purpose |
|---------|---------|
| `CalculateMonthlyTokenUsage()` | Sum of last 30 days |
| `CalculateMonthlyTokenUsageAsSubscriber()` | Sum since subscription renewal date |
| `AddTokenUsageEntry()` | Add/update single day |

**Key Features:**
- ⏰ Automatic deletion of old entries
- 🔄 Thread-safe with `ConcurrentDictionary`

---

## 🔌 Integration Points

### Where is `TokenDeductionService` used?

#### **PageStoreController.cs** - Flashcard Generation
```csharp
public async Task<GenerateFlashCardResponse?> GenerateFlashCard([FromBody] GenerateFlashCardRequest request)
{
    // Check: Can user generate the flashcards?
    if (!_tokenDeductionService.CanAffordTokens(
        _sessionUser.UserId, 
        request.Text, 
        TokenDeductionService.GenerationType.Flashcards))
    {
        return new GenerateFlashCardResponse(..., FrontendMessageKeys.Error.Ai.InsufficientTokens);
    }
    
    // Generation runs
    var aiFlashCard = new AiFlashCard(_aiUsageLogRepo);
    var flashcards = await aiFlashCard.Generate(...);
    
    // Tokens are deducted in AiFlashCard.Generate() via AiUsageLogRepo
}
```
📍 `src/Backend.Api/Api/NuxtUI/Stores/PageStoreController.cs:249-286`

#### **AiCreatePageController.cs** - Page & Wiki Generation
```csharp
// 3 Endpoints for:
// 1. CreatePage() - Generate single page
// 2. RegeneratePageContent() - Rewrite content  
// 3. GenerateWiki() - Wiki with subpages

// Each endpoint:
if (!_tokenDeductionService.CanAffordTokens(
    _sessionUser.UserId, 
    request.ModelId, 
    request.Prompt, 
    generationType)) // PageShort/Medium/Long or WikiWithSubpages
{
    return new AiCreatePageResponse(..., "insufficient_tokens");
}
```
📍 `src/Backend.Api/Api/NuxtUI/Components/Page/AI/AiCreatePageController.cs`

#### **UserStoreController.cs** - Balance Query
```csharp
var balance = _tokenDeductionService.GetTotalTokenBalance(_sessionUser.UserId);
```
📍 `src/Backend.Api/Api/NuxtUI/Stores/UserStoreController.cs:222`

#### **Stripe WebhookEventHandler.cs** - Subscription Events
```csharp
// On Stripe Success/Renewal:
_tokenDeductionService.GrantInitialSubscriptionTokens(user.Id);      // New subscription
_tokenDeductionService.RefreshMonthlySubscriptionTokens(user.Id);    // Monthly renewal
```
📍 `src/Backend.Core/Domain/Stripe/WebhookEventHandler.cs:209-214`

---

## 💾 Database Schema

### ai_usage_log Table
```sql
CREATE TABLE ai_usage_log (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    User_id INT NOT NULL,                    -- User who used the AI
    Page_id INT NOT NULL,                    -- Page where generation occurred
    TokenIn INT NOT NULL,                    -- Input tokens from API
    TokenOut INT NOT NULL,                   -- Output tokens from API
    DateCreated DATETIME NOT NULL,           -- UTC timestamp
    Model VARCHAR(255) NOT NULL,             -- Model ID (e.g., "claude-3-opus")
    FOREIGN KEY (User_id) REFERENCES user(Id),
    FOREIGN KEY (Page_id) REFERENCES page(Id),
    INDEX idx_user_date (User_id, DateCreated),
    INDEX idx_model (Model)
);
```

### aimodelwhitelist Table
```sql
CREATE TABLE aimodelwhitelist (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    ModelId VARCHAR(255) UNIQUE NOT NULL,                 -- e.g., "claude-3-opus"
    DisplayName VARCHAR(255) NOT NULL,                    -- "Claude 3 Opus"
    IsEnabled TINYINT(1) DEFAULT 1,                       -- Enabled/Disabled
    Provider VARCHAR(50) NOT NULL,                        -- "Anthropic" or "OpenAI"
    InputPricePerMillion DECIMAL(10,8) NOT NULL,         -- USD per 1M input tokens
    OutputPricePerMillion DECIMAL(10,8) NOT NULL,        -- USD per 1M output tokens
    TokenCostMultiplier DECIMAL(5,2) DEFAULT 1,          -- e.g., 1.2x for expensive models
    DateCreated DATETIME NOT NULL,
    INDEX idx_model_date (ModelId, DateCreated)
);
```

### user Table (relevant fields)
```sql
ALTER TABLE user ADD COLUMN (
    SubscriptionTokensBalance INT DEFAULT 0,    -- Tokens from active subscription
    PaidTokensBalance INT DEFAULT 0              -- Purchased tokens
);
```

---

## 🔄 Token Flow Example: Flashcard Generation

```
1. Frontend sends request to /apiVue/PageStore/GenerateFlashcard/
   ├─ pageId, text
   
2. PageStoreController.GenerateFlashCard()
   ├─ TokenDeductionService.CanAffordTokens(userId, text, Flashcards)
   │  ├─ EstimateTokens(text) → Input tokens
   │  ├─ GetEstimatedOutputTokens(Flashcards) → 800 output tokens
   │  ├─ GetTokenCostMultiplier(modelId) → e.g., 1.0 for Claude
   │  ├─ Calculation: (Input + 800) * 1.0 = Cost
   │  └─ Check: currentBalance >= costs (with -10000 buffer)
   │
   └─ If OK: AiFlashCard.Generate() → Anthropic API call

3. AiFlashCard.Generate() → Anthropic response with tokens
   └─ AiUsageLogRepo.AddUsage(response, userId, pageId)
      ├─ Create AiUsageLog with InputTokens, OutputTokens, Model
      ├─ Save to ai_usage_log table
      └─ TokenDeductionService.DeductTokens(userId, modelId, inputTokens, outputTokens)
         ├─ Calculate: totalTokens * TokenCostMultiplier
         ├─ Deduct: first from SubscriptionTokensBalance, then PaidTokensBalance
         ├─ Update user.SubscriptionTokensBalance and/or PaidTokensBalance
         ├─ Update ExtendedUserCache
         └─ Log: "Deducted X tokens from user Y"

4. Frontend receives flashcards → User can accept/reject them
```

---

## 📊 Analytics Features

### Cost Analysis Queries

**Costs per model (last 30 days):**
```csharp
var costSummary = aiUsageLogRepo.GetCostSummaryByModel(
    fromDate: DateTime.UtcNow.AddDays(-30)
);
// Returns: ModelId, TotalInputTokens, TotalOutputTokens, TotalCostUsd, UsageCount
```

**Detailed logs with costs:**
```csharp
var usage = aiUsageLogRepo.GetUsageWithCosts(
    fromDate: startDate,
    toDate: endDate,
    userId: specificUser
);
// Returns: Id, UserId, PageId, TokenIn/Out, DateCreated, Model, DisplayName, InputCostUsd, OutputCostUsd, TotalCostUsd
```

**Total costs & requests:**
```csharp
var totalCosts = aiUsageLogRepo.GetTotalCosts();
// Returns: TotalInputTokens, TotalOutputTokens, TotalCostUsd, TotalRequests
```

---

## 🎛️ Token Cost Multiplier

**Purpose:** Different AI models have different cost calculations

**Examples:**
- Claude 3 Opus: TokenCostMultiplier = 1.5  (50% more expensive)
- Claude 3 Sonnet: TokenCostMultiplier = 1.0 (standard)
- GPT-4o: TokenCostMultiplier = 0.8 (20% cheaper)

**During deduction:**
```csharp
// Raw API tokens: Input=100, Output=50 → Total=150
// Multiplier: 1.5
// Deducted: Math.Ceiling(150 * 1.5) = 225 tokens
```

---

## 🚀 Key Features

✅ **Affordability check BEFORE generation** - Prevents over-generation  
✅ **Dual token balance system** - Subscription vs. purchased tokens  
✅ **Model-aware cost calculation** - Different models, different costs  
✅ **Automatic logging & deduction** - Coupled in `AiUsageLogRepo.AddUsage()`  
✅ **Optimistic locking** - Race condition prevention  
✅ **Comprehensive analytics** - Cost summary by model, total costs, etc.  
✅ **Cache integration** - `ExtendedUserCache.MonthlyTokenUsage` for performance  
✅ **Stripe integration** - Token grants on subscription events  
✅ **Audit trail** - Detailed logs with timestamps & models  

---

## 📝 Notes for Frontend Integration

If you want to track AI usage in the frontend:

1. **Backend returns token info** - Extend response models:
   ```csharp
   public record GenerateFlashCardResponse(
       List<FlashCard> Flashcards, 
       string? MessageKey,
       int TokensUsed,           // NEW
       decimal CostUsd           // NEW
   );
   ```

2. **Frontend displays usage** - Example:
   ```vue
   <!-- "This generation cost 325 tokens ($0.12)" -->
   ```

3. **User balance dashboard** - Example:
   ```vue
   <!-- Subscription: 45,000 / 100,000 tokens
        Purchased: 10,000 tokens
        Total: 55,000 tokens -->
   ```
