﻿using Business.Handlers.Places.Queries;
using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers.SuggestionServices
{
    public class AprioriAlgorithmSystem : ISuggestionSystem
    {
        public IDataResult<List<List<string>>> GetAprioriAlgorithm(List<List<string>> allPreferences)
        {
            double minSupport = 0.1; // min support value
            double minConfidence = 0.5; // min confidence value

            // find frequent itemsets using the apriori algorithm
            List<List<string>> frequentItemsets = Apriori(allPreferences, minSupport);

            // Calculate Confidence Value
            List<AssociationRuleModel> associationRules = CalculateConfidence(frequentItemsets, allPreferences, minConfidence);

            var result = GetStrongestRecommendations(associationRules, 15);

            if (result != null && result.Count > 0)
                return new SuccessDataResult<List<List<string>>>(result, "Apriori Algorithm is done.");
            return new ErrorDataResult<List<List<string>>>("Apriori Algorithm is not done.");
        }

        // associationRules listesini kullanarak en güçlü önerileri getiren metod
        private List<List<string>> GetStrongestRecommendations(List<AssociationRuleModel> associationRules, int topN)
        {
            // En yüksek confidence değeri
            double maxConfidence = associationRules.Max(rule => rule.Confidence);

            // En yüksek confidence değerine sahip antecedent verilerini seçme
            // Kullanıcıya en iyi topN öneriyi sunma
            var bestRules = associationRules.Where(rule => rule.Consequent.Count > 0)
                                            .GroupBy(rule => rule.Antecedent)
                                            .Select(group => group.OrderByDescending(rule => rule.Confidence).First())
                                            .OrderByDescending(rule => rule.Confidence)
                                            .Take(topN)
                                            .ToList().Distinct();

            List<List<string>> recommandationList = new List<List<string>>();
            foreach (var rule in bestRules)
            {
                var antecedent = rule.Antecedent;
                var consequent = rule.Consequent;
                var confidence = rule.Confidence;

                // Antecedent ve consequent değerlerini birleştirerek öneri yapma
                var recommendation = antecedent.Concat(consequent).ToList();

                recommandationList.Add(recommendation);
            }

            return recommandationList;
        }
        private List<List<string>> Apriori(List<List<string>> transactions, double minSupport)
        {
            List<List<string>> frequentItemsets = new List<List<string>>();

            // 1. Create a C candidate itemset
            List<List<string>> candidates = GenerateInitialCandidates(transactions);

            // Itemset loop
            while (candidates.Count > 0)
            {
                // 2. Filtering the C candidate itemset with a support check
                List<List<string>> frequentCandidates = FilterCandidates(candidates, transactions, minSupport);

                // 3. Add the favorite itemset to the favorite itemset list
                frequentItemsets.AddRange(frequentCandidates);

                // 4. Generating the next candidate itemset from the C candidate itemset
                candidates = GenerateNextCandidates(frequentCandidates);

                // 5. Loop termination condition
                if (candidates.Count == 0)
                    break;
            }

            return frequentItemsets;
        }
        private List<List<string>> GenerateInitialCandidates(List<List<string>> transactions)
        {
            List<List<string>> candidates = new List<List<string>>();
            HashSet<string> uniqueItems = new HashSet<string>();

            // Adding all single items to the candidate itemset
            foreach (List<string> transaction in transactions)
            {
                foreach (string item in transaction)
                {
                    if (!string.IsNullOrEmpty(item) && !uniqueItems.Contains(item))
                    {
                        List<string> candidate = new List<string> { item };
                        candidates.Add(candidate);
                        uniqueItems.Add(item);
                    }
                }
            }

            return candidates;
        }
        private List<List<string>> FilterCandidates(List<List<string>> candidates, List<List<string>> transactions, double minSupport)
        {
            Dictionary<List<string>, int> candidateCounts = new Dictionary<List<string>, int>();

            // Calculate the support count for each candidate item set
            foreach (List<string> candidate in candidates)
            {
                int count = 0;

                foreach (List<string> transaction in transactions)
                {
                    if (IsSubset(candidate, transaction))
                    {
                        count++;
                    }
                }

                double support = (double)count / transactions.Count;
                if (support >= minSupport)
                {
                    candidateCounts.Add(candidate, count);
                }
            }

            //// Filter candidates by support value check
            List<List<string>> frequentCandidates = candidateCounts.Keys.ToList();

            return frequentCandidates;
        }
        private List<List<string>> GenerateNextCandidates(List<List<string>> frequentCandidates)
        {
            List<List<string>> nextCandidates = new List<List<string>>();

            if (frequentCandidates != null && frequentCandidates.Count > 0)
            {
                int k = frequentCandidates[0].Count + 1;

                // Combining frequency candidate itemsets
                for (int i = 0; i < frequentCandidates.Count - 1; i++)
                {
                    for (int j = i + 1; j < frequentCandidates.Count; j++)
                    {
                        List<string> candidate1 = frequentCandidates[i];
                        List<string> candidate2 = frequentCandidates[j];

                        // Merge is done if previous candidates have the same k-1 items
                        bool canMerge = true;
                        for (int m = 0; m < k - 2; m++)
                        {
                            if (candidate1[m] != candidate2[m])
                            {
                                canMerge = false;
                                break;
                            }
                        }

                        if (canMerge)
                        {
                            List<string> mergedCandidate = new List<string>(candidate1);
                            mergedCandidate.Add(candidate2[k - 2]);
                            nextCandidates.Add(mergedCandidate);
                        }
                    }
                }
            }

            return nextCandidates;
        }

        private List<AssociationRuleModel> CalculateConfidence(List<List<string>> frequentItemSets, List<List<string>> allPreferences, double minConfidence)
        {
            List<AssociationRuleModel> associationRules = new List<AssociationRuleModel>();

            foreach (List<string> itemSet in frequentItemSets)
            {
                List<List<string>> subsets = GetSubsets(itemSet);

                foreach (List<string> subset in subsets)
                {
                    List<string> complementSet = itemSet.Except(subset).ToList();

                    int itemSetSupportCount = GetSupportCount(allPreferences, itemSet);
                    int subsetSupportCount = GetSupportCount(allPreferences, subset);
                    int complementSetSupportCount = GetSupportCount(allPreferences, complementSet);

                    double confidence = (double)itemSetSupportCount / subsetSupportCount;
                    if (confidence >= minConfidence)
                    {
                        AssociationRuleModel rule = new AssociationRuleModel
                        {
                            Antecedent = subset,
                            Consequent = complementSet,
                            Support = (double)itemSetSupportCount / allPreferences.Count,
                            Confidence = confidence
                        };

                        associationRules.Add(rule);
                    }
                }
            }

            return associationRules;
        }

        private int GetSupportCount(List<List<string>> transactions, List<string> itemSet)
        {
            int count = 0;

            foreach (var transaction in transactions)
            {
                if (IsSubset(itemSet, transaction))
                {
                    count++;
                }
            }

            return count;
        }

        private List<List<string>> GetSubsets(List<string> itemSet)
        {
            List<List<string>> subsets = new List<List<string>>();
            int length = itemSet.Count;

            for (int i = 1; i < (1 << length); i++)
            {
                List<string> subset = new List<string>();

                for (int j = 0; j < length; j++)
                {
                    if ((i & (1 << j)) > 0)
                    {
                        subset.Add(itemSet[j]);
                    }
                }

                subsets.Add(subset);
            }

            return subsets;
        }

        private bool IsSubset(List<string> subset, List<string> superset)
        {
            foreach (string item in subset)
            {
                if (!superset.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
